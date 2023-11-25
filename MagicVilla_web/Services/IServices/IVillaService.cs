using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(VillaCreateDTO villa);
        Task<T> UpdateAsync<T>(VillaUpdateDTO villa);
        Task<T> DeleteAsync<T>(int id);
    }
}
