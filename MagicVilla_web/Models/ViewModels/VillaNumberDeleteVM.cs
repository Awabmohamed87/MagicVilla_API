using MagicVilla_web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_web.Models.ViewModels
{
    public class VillaNumberDeleteVM
    {
        public VillaNumberDTO VillaNumber { get; set; }
        [ValidateNever]
        public VillaDTO Villa { get; set; }
    }
}
