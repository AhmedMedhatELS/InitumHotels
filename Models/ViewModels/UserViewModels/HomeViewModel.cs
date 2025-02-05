using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.UserViewModels
{
    public class HomeViewModel
    {
        public ReservationFilter ReservationFilter { get; set; } = null!;
        public List<RoomsHomeView> RoomsHomeViews { get; set;} = [];
        public List<HotelBranch> HotelBranches { get; set; } = [];
        public List<RoomType> RoomTypes { get; set; } = [];

        public string? CustomerName { get; set; }
        public string? NationalId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
