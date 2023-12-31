using MagicVilla_web.Models;
using MagicVilla_web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using MagicVilla_Utility;
using System.Text.Json.Serialization;

namespace MagicVilla_web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse Response { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory HttpClient)
        {
            Response = new APIResponse();
            httpClient = HttpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest Request)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.Headers.Add("Accept", "application/json");
                httpRequestMessage.RequestUri = new Uri(Request.Url);
                if(Request.Data != null)
                {
                    httpRequestMessage.Content = new StringContent(JsonConvert.SerializeObject(Request.Data), Encoding.UTF8, "application/json");
                }
                switch(Request.ApiType)
                {
                    case SD.ApiType.Get:
                        httpRequestMessage.Method = HttpMethod.Get;break;
                    case SD.ApiType.Post:
                        httpRequestMessage.Method = HttpMethod.Post; break;
                    case SD.ApiType.Delete:
                        httpRequestMessage.Method = HttpMethod.Delete; break;
                    case SD.ApiType.Put:
                        httpRequestMessage.Method = HttpMethod.Put; break;
                    default:
                        httpRequestMessage.Method = HttpMethod.Patch; break;
                }
                HttpResponseMessage httpResponseMessage = null;
                if (!string.IsNullOrEmpty(Request.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",Request.Token);
                }
                httpResponseMessage = await client.SendAsync(httpRequestMessage);

                var apiContent = await httpResponseMessage.Content.ReadAsStringAsync();
                try
                {
                    APIResponse ApiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    if(ApiResponse!=null && (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest ||
                        httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound))
                    {
                        ApiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var response = JsonConvert.DeserializeObject<T>(res);
                        return response;
                    }
                }
                catch (Exception)
                {
                    var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return APIResponse;

                }
                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponse;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string>() { Convert.ToString(ex.Message) },
                    IsSuccess = false,
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponse = JsonConvert.DeserializeObject<T>(res);
                return apiResponse;
            }
        }
    }
}
