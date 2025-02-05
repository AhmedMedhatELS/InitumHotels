using DataAccess.Repository.IRepository;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels.ReservationViewModel;
using Models;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
namespace Shared
{
    public class ReservationHelper(IUnitOfWork unitOfWork) : BaseService(unitOfWork)
    {
        public DiscountDetail GetDiscountAmount(int Total,string NationalId, string? UserID = null)
        {
            DiscountDetail discountDetail = new()
            {
                IsDiscounted = _unitOfWork.Repository<Reservation>().Get(
                e => e.NationalId == NationalId ||
                (UserID != null && e.ApplicationUserId == UserID)).ToList().Count > 0
            };

            if (discountDetail.IsDiscounted) 
            {
                discountDetail.DiscountAmount = (int)(Total * 0.05);
                discountDetail.Total = Total - discountDetail.DiscountAmount;
            }
            else
            {
                discountDetail.Total = Total;
                discountDetail.DiscountAmount = 0;
            }

            return discountDetail; 
        }
    
        public Dictionary<int,string>? GetRoomIDs(string RoomsData)
        {
            Dictionary<int, string> RoomsResult = [];

            var pairs = RoomsData.Split('-');

            //must be even
            if (pairs.Length % 2 != 0)  return null;
            

            for (int i = 0; i < pairs.Length; i += 2) {

                bool IsItRoomId = int.TryParse(pairs[i], out int roomId);
                bool IsDataExcits = !string.IsNullOrEmpty(pairs[i + 1]); 

                if(!IsDataExcits || !IsItRoomId)
                    return null;

                RoomsResult.Add(roomId, pairs[i + 1]);
            }

            return RoomsResult;
        }

        public bool IsAllTheRoomExsitsAndInBranch(int branchId,List<int> roomIds)
        {
            var hotelBranch = _unitOfWork.Repository<HotelBranch>().GetOne(
                e => e.HotelBranchId == branchId && !e.IsDeleted,r => r.Rooms);

            if(hotelBranch == null) return false;

            return roomIds.All(id => hotelBranch.Rooms.Any(r => r.RoomId == id));
        }

        public List<ReservationRoomView>? GetRoomsData(Dictionary<int, string> Rooms) 
        {
            List<ReservationRoomView> view = [];

            foreach(var roomId in Rooms.Keys)
            {
                var room = _unitOfWork.Repository<Room>().GetOne(e => e.RoomId == roomId);

                if(room == null) return null;

                view.Add(new ReservationRoomView 
                {
                    RoomId = roomId,
                    RoomName = room.RoomName,
                    Price = room.PricePerNight,
                });

                var pairs = Rooms[roomId].Split('/');

                foreach(var pair in pairs)
                {
                    var finailPairs = pair.Split(',');

                    if(finailPairs.Length != 2) return null;

                    var IsAdultNum = int.TryParse(finailPairs[0], out int adults);
                    var IsChildrenNum = int.TryParse(finailPairs[1], out int childreen);

                    view.Last().RoomEntities.Add(new RoomEntity
                    {
                        Adults = adults,
                        Children = childreen
                    });
                }
            }

            return view;
        }

        public int TotalPrice(List<ReservationRoomView> roomsData, DateTime CheckIn, DateTime CheckOut)
        {
            int total = 0;

            int numberNights = (CheckOut.Date - CheckIn.Date).Days + 1;

            foreach (var roomData in roomsData)
                total += roomData.Price * roomData.RoomEntities.Count * numberNights;

            return total;
        }

        public List<ReservationsList>? GetAllReservations(string? userId = null)
        {
            var reservations = _unitOfWork.Repository<Reservation>().Get(
                e => userId == null || e.ApplicationUserId == userId,
                e => e.Rooms, b => b.HotelBranch);

            var rooms = _unitOfWork.Repository<Room>().Get(null);

            if (reservations == null || rooms == null) return null;

            List<ReservationsList> view = [];

            foreach (var reservation in reservations)
            {
                ReservationsList entity = new()
                {
                    ReservationId = reservation.ReservationId,
                    DiscountOn = reservation.DiscountOn,
                    NationalId = reservation.NationalId,
                    CustomerName = reservation.CustomerName,
                    Email = reservation.Email,
                    PhoneNumber = reservation.PhoneNumber,
                    Total = reservation.Total,
                    HotelBranchName = reservation.HotelBranch.HotelBranchName,
                    CheckInDate = reservation.CheckInDate.ToString("M/d/yyyy"),
                    CheckOutDate = reservation.CheckOutDate.ToString("M/d/yyyy"),
                    Status = reservation.Status.ToString()
                };

                Dictionary<int, string> ReservationRooms = [];

                #region get rooms orginal data
                foreach (var room in reservation.Rooms)
                    ReservationRooms.Add(room.RoomId, room.RoomDetails);

                var ReservationRoomsOrgainalData = GetRoomsData(ReservationRooms) ?? [];
                #endregion
                foreach (var room in ReservationRoomsOrgainalData)
                {
                    if (room == null) continue;

                    var newRoom = new RoomsList { Name = room.RoomName };

                    entity.Rooms.Add(newRoom);

                    foreach (var eRoom in room.RoomEntities)
                        newRoom.RoomNumbers.Add(
                            $"Adults Number is {eRoom.Adults}  |  Children Number is {eRoom.Children}");
                }

                entity.RoomsJson = JsonConvert.SerializeObject(entity.Rooms);

                view.Add(entity);
            }

            return view;
        }
    }
}
