using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels.AdminViewModels
{
    public class NewRoom
    {
        [Required]
        public string RoomName { get; set; } = null!;
        [Required]
        [StringLength(150, ErrorMessage = "Description cannot exceed 150 characters.")]
        public string Description { get; set; } = null!;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of beds must be greater than 0.")]
        public int NumBeds { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of Adults must be greater than 0.")]
        public int MaxAdults { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The number of Children must be greater than 0.")]
        public int MaxChildren { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public int PricePerNight { get; set; }
        [Required]
        public int RoomTypeId { get; set; }
        [ValidateNever]
        public List<IFormFile> ImagesFiles { get; set; } = [];
        [ValidateNever]
        public List<RoomType> roomTypes { get; set; } = [];
    }
}
