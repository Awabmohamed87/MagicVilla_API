using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(VillaCreateDTO villa, string token);
        Task<T> UpdateAsync<T>(VillaUpdateDTO villa, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
