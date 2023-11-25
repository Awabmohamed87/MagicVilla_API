using AutoMapper;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagicVilla_web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> list = new List<VillaDTO>();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateVilla()
        {
            return View(new VillaCreateDTO());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villa)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(villa);
                if(response != null && response.IsSuccess) 
                {
                    TempData["success"] = "Villa Created Successfully";
                    return RedirectToAction(nameof(Index));                
                }
            }
            TempData["failed"] = "Error Deleting Villa";
            return View(villa);
        }
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if(response != null && response.IsSuccess )
            {

                VillaDTO villaToUpdate = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDTO>(villaToUpdate));
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villa)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(villa);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["failed"] = "Error Deleting Villa";
            return View(villa);
        }
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetAsync<APIResponse>(villaId);
            if (response != null && response.IsSuccess)
            {

                VillaDTO villaToDelete = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(villaToDelete);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO villa)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(villa.Id);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Successfully Deleted";
                return RedirectToAction(nameof(Index));
            }
            TempData["failed"] = "Error Deleting Villa";
            return View(villa);
        }
    }
}
