using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.UserViewModels
{
    public class ReservationFilter
    {
        public int BranchId { get; set; }
        public DateTime? CheckInDate { get; set; } = null;
        public DateTime? CheckOutDate { get; set; } = null;
    }
}
