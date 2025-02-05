using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.AdminViewModels
{
    public class HotelSimplelist
    {
        public int HotelBranchId {  get; set; }
        public string HotelName { get; set; } = null!;
        public List<RoomSimpleList> HotelAssignedRooms { get; set; } = [];
    }
}
