using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Models
{
    public class Reservation
    {
        #region Properities
        public int ReservationId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string CustomerName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool DiscountOn { get; set; } = false;
        public int Total {  get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        #endregion

        #region Foreign Keys
        public string? ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int HotelBranchId { get; set; }
        public HotelBranch HotelBranch { get; set; } = null!;
        #endregion

        #region Relations
        public ICollection<ReservationRoom> Rooms { get; set; } = [];
        #endregion
    }
}
