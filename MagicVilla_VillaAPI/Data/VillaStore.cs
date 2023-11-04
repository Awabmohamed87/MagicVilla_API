using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> VillasList = new List<VillaDTO>() 
        { 
            new VillaDTO { Id = 1, Name = "Pool View" , sqft = 100, Occupancy = 4},
            new VillaDTO { Id = 2, Name = "Beach View" , sqft = 300, Occupancy = 3}
        };

    }
}
