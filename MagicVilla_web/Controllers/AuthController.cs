using MagicVilla_Utility;
using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagicVilla_web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService; 
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO loginRequestDTO = new LoginRequestDTO();
            return View(loginRequestDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            APIResponse response = await authService.LoginAsync<APIResponse>(loginRequestDTO);
            if(response != null && response.IsSuccess)
            {
                LoginResponseDTO loginResponseDTO = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                HttpContext.Session.SetString(SD.SessionToken, loginResponseDTO.Token); 
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
            return View(loginRequestDTO);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegistrationRequestDTO loginRequestDTO = new RegistrationRequestDTO();
            return View(loginRequestDTO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO Request)
        {
            APIResponse Response =  await authService.RegisterAsync<APIResponse>(Request);
            if(Response != null && Response.IsSuccess) 
            {
                return RedirectToAction("Login");
            }
            return View(Request);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");            
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
