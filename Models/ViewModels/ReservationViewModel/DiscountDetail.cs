using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.ReservationViewModel
{
    public class DiscountDetail
    {
        public bool IsDiscounted { get; set; } = false;
        public int Total {  get; set; }
        public int DiscountAmount { get; set; }
    }
}
