using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.AdminViewModels
{
    public class HotelBranchView
    {
        public HotelBranch NewHotelBranch { get; set; } = null!;

        public List<HotelBranch> ExsitHotelBranches { get; set; } = [];
    }
}
