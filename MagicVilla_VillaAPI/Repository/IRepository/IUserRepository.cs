using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepository : IRepository<LocalUser>
    {
        bool IsUniqueUser(string Email);
        Task<LoginResponseDTO> Login(LoginRequestDTO Request);
        Task<LocalUser> Register(RegistrationRequestDTO Request);
    }
}
