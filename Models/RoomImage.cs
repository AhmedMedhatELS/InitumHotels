using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoomImage
    {
        #region Properities
        public int RoomImageId { get; set; }
        public string ImageName { get; set; } = null!;
        #endregion

        #region Foreign Keys
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        #endregion

        #region Relations

        #endregion
    }
}
