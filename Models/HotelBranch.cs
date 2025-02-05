using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HotelBranch
    {
        #region Properities
        public int HotelBranchId { get; set; }
        public string HotelBranchName { get; set; } = null!;
        public string HotelBranchLocation { get; set; } = null!;
        public string HotelBranchMapsLink { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Foreign Keys

        #endregion

        #region Relations
        public ICollection<Room> Rooms { get; set; } = [];
        public ICollection<HotelRoom> HotelRooms { get; set;} = [];
        public ICollection<Reservation> Reservations { get; set; } = [];
        #endregion
    }
}
