using DataAccess.Repository.IRepository;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.ViewModels.AdminViewModels;
using Models.ViewModels.UserViewModels;

namespace Shared
{
    public class RoomHelper(IUnitOfWork unitOfWork) : BaseService(unitOfWork)
    {
        public List<RoomType> GetRoomTypes() =>
            _unitOfWork.Repository<RoomType>().Get(e => !e.IsDeleted).ToList();

        public List<RoomSimpleList> RoomList()
        {
            var rooms = _unitOfWork.Repository<Room>().Get(null);

            List<RoomSimpleList> roomList = [];

            foreach (var room in rooms)
                roomList.Add(new RoomSimpleList
                {
                    Id = room.RoomId,
                    Name = room.RoomName
                });

            return roomList;
        }

        public List<RoomsHomeView> GetRoomsHomeViews(List<Room> rooms)
        {
            List<RoomsHomeView> view = [];

            foreach (var room in rooms)
            {
                room.Images = _unitOfWork.Repository<RoomImage>().Get(
                    e => e.RoomId == room.RoomId).ToList();

                room.RoomType = _unitOfWork.Repository<RoomType>().GetOne(
                    e => e.RoomTypeId == room.RoomTypeId) ?? new RoomType();

                view.Add(new RoomsHomeView
                {
                    RoomId = room.RoomId,
                    MaxAdults = room.MaxAdults,
                    MaxChildren = room.MaxChildren,
                    NumBeds = room.NumBeds,
                    PricePerNight = room.PricePerNight,
                    Description = room.Description,
                    RoomName = room.RoomName,
                    RoomTypeName = room.RoomType.Name,
                    RoomImages = room.Images.Select(e => e.ImageName).ToList()
                });
            }

            return view;
        }
    }
}
