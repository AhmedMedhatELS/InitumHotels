using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.UserViewModels
{
    public class RoomsHomeView
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int NumBeds { get; set; }
        public int MaxAdults { get; set; }
        public int MaxChildren { get; set; }
        public int PricePerNight { get; set; }
        public string RoomTypeName { get; set; } = null!;
        public List<string> RoomImages { get; set; } = [];
    }
}
