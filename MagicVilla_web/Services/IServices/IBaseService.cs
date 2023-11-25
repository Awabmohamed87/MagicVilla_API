using MagicVilla_web.Models;

namespace MagicVilla_web.Services.IServices
{
    public interface IBaseService
    {
        APIResponse Response { get; set; }
        Task<T> SendAsync<T>(APIRequest Request);
    }
}
