using DataAccess.Repository.IRepository;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.ViewModels.AdminViewModels;

namespace Shared
{
    public class HotelHelper(IUnitOfWork unitOfWork) : BaseService(unitOfWork)
    {
        public List<HotelSimplelist> HotelList()
        {
            var hotels = _unitOfWork.Repository<HotelBranch>().Get(
                e => !e.IsDeleted, r => r.Rooms);

            List<HotelSimplelist> hotelsList = [];

            foreach (var hotel in hotels)
            {
                hotelsList.Add(new HotelSimplelist
                {
                    HotelBranchId = hotel.HotelBranchId,
                    HotelName = hotel.HotelBranchName
                });

                foreach (var room in hotel.Rooms)
                    hotelsList.Last().HotelAssignedRooms.Add(new RoomSimpleList
                    {
                        Id = room.RoomId,
                        Name = room.RoomName
                    });
            }

            return hotelsList;
        }

        public List<HotelBranch> GetAllBranchs() => 
            _unitOfWork.Repository<HotelBranch>().Get(e => !e.IsDeleted).ToList();
    }
}
