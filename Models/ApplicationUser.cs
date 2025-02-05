using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        #region Properities
        public string? FullName { get; set; }
        public string? ProfileImage { get; set; }
        public string? NationalId { get; set; }
        #endregion

        #region Foreign Keys

        #endregion

        #region Relations
        public ICollection<Reservation> Reservations { get; set; } = [];
        #endregion
    }
}
