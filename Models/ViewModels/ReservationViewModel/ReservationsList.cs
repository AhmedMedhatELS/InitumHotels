using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models.ViewModels.ReservationViewModel
{
    public class ReservationsList
    {
        #region model fade
        public string NationalId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string HotelBranchName { get; set; } = null!;
        public bool DiscountOn { get; set; } = false;

        public List<RoomsList> Rooms { get; set; } = [];
        public string RoomsJson { get; set; } = null!;
        #endregion
        #region both table and model fade
        public int ReservationId { get; set; }
        public string CheckInDate { get; set; } = null!;
        public string CheckOutDate { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public int Total { get; set; }
        public string Status { get; set; } = null!;
        #endregion
    }
}
