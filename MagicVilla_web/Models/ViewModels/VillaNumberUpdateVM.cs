using MagicVilla_web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_web.Models.ViewModels
{
    public class VillaNumberUpdateVM
    {
        public VillaNumberUpdateDTO VillaNumber { get; set; }
        [ValidateNever]
        public List<SelectListItem> VillasNames { get; set; }
    }
}
