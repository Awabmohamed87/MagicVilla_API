using System.ComponentModel.DataAnnotations;

namespace MagicVilla_web.Models.DTO
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Occupancy { get; set; }
        public int sqft { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
