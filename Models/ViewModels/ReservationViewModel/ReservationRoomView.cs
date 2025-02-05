using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.ReservationViewModel
{
    public class ReservationRoomView
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int Price { get; set; }
        public List<RoomEntity> RoomEntities { get; set; } = [];
    }
}
