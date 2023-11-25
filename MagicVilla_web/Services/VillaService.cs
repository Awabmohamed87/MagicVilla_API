using MagicVilla_web.Models.DTO;
using MagicVilla_web.Services.IServices;
using MagicVilla_web.Models;
using MagicVilla_Utility;

namespace MagicVilla_web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClient;
        private string _villaUrl;
        public VillaService(IHttpClientFactory HttpClient, IConfiguration configuration) : base(HttpClient)
        {
            _httpClient = HttpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi")??"";
        }

        public Task<T> CreateAsync<T>(VillaCreateDTO villa) => SendAsync<T>(new APIRequest() { 
            ApiType = SD.ApiType.Post,
            Data = villa,
            Url = _villaUrl + "/api/VillaAPI"
        });
        public Task<T> DeleteAsync<T>(int id) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Delete,
            Url = _villaUrl + "/api/VillaAPI/" + id
        });

        public Task<T> GetAllAsync<T>() => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Get,
            Url = _villaUrl + "/api/VillaAPI"
        });

        public Task<T> GetAsync<T>(int id) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Get,
            Url = _villaUrl + "/api/VillaAPI/" + id
        });

        public Task<T> UpdateAsync<T>(VillaUpdateDTO villa) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Put,
            Data = villa,
            Url = _villaUrl + "/api/VillaAPI/" + villa.Id   
        });
    }
}
