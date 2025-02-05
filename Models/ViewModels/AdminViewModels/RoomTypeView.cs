using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.AdminViewModels
{
    public class RoomTypeView
    {
        public string NewRoomType { get; set; } = null!;

        public List<RoomType> RoomTypes { get; set; } = [];
    }
}
