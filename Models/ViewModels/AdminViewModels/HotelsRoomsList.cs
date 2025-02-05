using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.AdminViewModels
{
    public class HotelsRoomsList
    {
        public List<RoomSimpleList> RoomSimpleList { get; set; } = [];
        public List<HotelSimplelist> HotelBranches { get; set; } = [];
    }
}
