using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.ReservationViewModel
{
    public class RoomsList
    {
        public string Name { get; set; } = null!;
        public List<string> RoomNumbers { get; set; } = [];
    }
}
