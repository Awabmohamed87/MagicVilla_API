using MagicVilla_web.Models.DTO;
using MagicVilla_web.Services.IServices;
using MagicVilla_web.Models;

namespace MagicVilla_web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        string url;
        public AuthService(IHttpClientFactory HttpClient, IConfiguration configuration) : base(HttpClient)
        {
            url = configuration.GetValue<string>("ServiceUrls:VillaApi");
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO Request)
        {
            return await SendAsync<T>(new APIRequest()
            {
                Url = url + "/api/v1/UserAPI/Login",
                ApiType = MagicVilla_Utility.SD.ApiType.Post,
                Data = Request
            });
        }

        public async Task<T> RegisterAsync<T>(RegistrationRequestDTO Request)
        {
            return await SendAsync<T>(new APIRequest()
            {
                Url = url + "/api/v1/UserAPI/Register",
                ApiType = MagicVilla_Utility.SD.ApiType.Post,
                Data = Request
            });
        }
    }
}
