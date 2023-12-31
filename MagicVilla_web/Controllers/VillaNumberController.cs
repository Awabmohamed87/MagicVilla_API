using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagicVilla_web.Models.ViewModels;
using MagicVilla_web.Services;
using MagicVilla_web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace MagicVilla_web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService VillaNumberService;
        private readonly IVillaService VillaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService, IMapper mapper)
        {
            VillaNumberService = villaNumberService;
            VillaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var response = await VillaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var data = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            return View(data);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            var response = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var Villas = JsonConvert.DeserializeObject<List<Villa>>(Convert.ToString(response.Result));
            return View(new VillaNumberCreateVM
            {
                VillaNumber = new VillaNumberCreateDTO(),
                VillasNames = Villas.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM villa)
        {
            if (ModelState.IsValid)
            {
                var response = await VillaNumberService.CreateAsync<APIResponse>(villa.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if(response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErroeMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            var res = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var Villas = JsonConvert.DeserializeObject<List<Villa>>(Convert.ToString(res.Result));
            return View(new VillaNumberCreateVM
            {
                VillaNumber = new VillaNumberCreateDTO(),
                VillasNames = Villas.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
            });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            var response = await VillaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            var response2 = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var Villas = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response2.Result));
            return View(new VillaNumberUpdateVM
            {
                VillaNumber = JsonConvert.DeserializeObject<VillaNumberUpdateDTO>(Convert.ToString(response.Result)),
                VillasNames = Villas.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villa)
        {
            if (ModelState.IsValid)
            {
                var response = await VillaNumberService.UpdateAsync<APIResponse>(villa.VillaNumber, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if(response.ErrorMessages.Count > 0)
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault()); 
                }
            }
            var res = await VillaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            var Villas = JsonConvert.DeserializeObject<List<Villa>>(Convert.ToString(res.Result));
            return View(new VillaNumberCreateVM
            {
                VillaNumber = new VillaNumberCreateDTO(),
                VillasNames = Villas.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList()
            });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            var response = await VillaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionToken));
            var VillaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
            var response2 = await VillaService.GetAsync<APIResponse>(VillaNumber.VillaId, HttpContext.Session.GetString(SD.SessionToken));
            var Villa = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response2.Result));
            return View(new VillaNumberDeleteVM { 
            VillaNumber = VillaNumber,
            Villa = Villa
            } );
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM villaNumberDeleteVM)
        {
            if (ModelState.IsValid)
            {
                var response = await VillaNumberService.DeleteAsync<APIResponse>(villaNumberDeleteVM.VillaNumber.VillaNo, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(villaNumberDeleteVM);
        }
    }
}
