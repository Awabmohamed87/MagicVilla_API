using MagicVilla_web.Models;
using MagicVilla_web.Models.DTO;

namespace MagicVilla_web.Services.IServices
{
    public interface IAuthService
    {
        public Task<T> LoginAsync<T>(LoginRequestDTO Request);
        public Task<T> RegisterAsync<T>(RegistrationRequestDTO Request);
    }
}
