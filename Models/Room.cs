using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Room
    {
        #region Properities
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int NumBeds { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public int PricePerNight { get; set; }
        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Foreign Keys
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;
        #endregion

        #region Relations
        public ICollection<RoomImage> Images { get; set; } = [];
        public ICollection<HotelBranch> HotelBranches { get; set; } = [];
        public ICollection<HotelRoom> HotelRooms { get; set; } = [];
        public ICollection<ReservationRoom> ReservationsRooms { get; set; } = [];
        #endregion
    }
}
