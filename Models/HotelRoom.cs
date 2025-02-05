using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class HotelRoom
    {
        #region Properities
        public int HotelRoomId { get; set; }
        #endregion

        #region Foreign Keys
        public int HotelBranchId { get; set; }
        public HotelBranch HotelBranch { get; set; } = null!;

        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        #endregion

        #region Relations

        #endregion
    }
}
