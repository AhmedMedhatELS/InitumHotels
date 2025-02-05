using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReservationRoom
    {
        #region Properities
        public int ReservationRoomId {  get; set; }
        public string RoomDetails { get; set; } = null!;
        #endregion

        #region Foreign Keys
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        #endregion

        #region Relations

        #endregion
    }
}
