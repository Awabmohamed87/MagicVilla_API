using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_web.Models.DTO
{
    public class VillaCreateDTO
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        
        public int Occupancy { get; set; }
        public int sqft { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

        public string Amenity { get; set; }
    }
}
