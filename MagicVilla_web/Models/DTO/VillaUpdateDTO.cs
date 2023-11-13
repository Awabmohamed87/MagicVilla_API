using System.ComponentModel.DataAnnotations;

namespace MagicVilla_web.Models.DTO
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public int sqft { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
