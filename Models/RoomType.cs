using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomType
    {
        #region Properities
        public int RoomTypeId { get; set; }
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        #endregion

        #region Foreign Keys

        #endregion

        #region Relations
        public ICollection<Room> Rooms { get; set; } = [];
        #endregion
    }
}
