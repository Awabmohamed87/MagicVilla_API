using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;
using MagicVilla_Utility;
using MagicVilla_web.Services.IServices;
using static MagicVilla_Utility.SD;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace MagicVilla_web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private string _villaUrl;
        public VillaNumberService(IHttpClientFactory HttpClient,IConfiguration configuration) : base(HttpClient)
        {
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi")??"";
        }

        public Task<T> CreateAsync<T>(VillaNumberCreateDTO villa, string token) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Post,
            Data = villa,
            Url =  _villaUrl + "/api/v1/VillaNumberAPI",
            Token = token
        });

        public Task<T> DeleteAsync<T>(int id, string token) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Delete,
            Url = _villaUrl + "/api/v1/VillaNumberAPI/" + id ,
            Token = token
        });

        public Task<T> GetAllAsync<T>(string token) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Get,
            Url = _villaUrl + "/api/v1/VillaNumberAPI",
            Token = token
        });

        public Task<T> GetAsync<T>(int id, string token) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Get,
            Url = _villaUrl + "/api/v1/VillaNumberAPI/" + id,
            Token = token
        });

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO villa, string token) => SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.Put,
            Data = villa,
            Url = _villaUrl + "/api/v1/VillaNumberAPI/" + villa.VillaNo,
            Token = token
        });
    }
}
