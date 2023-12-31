using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : Repository<LocalUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly string secretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string Email)
        {
            var user = _db.LocalUsers.FirstOrDefault(user => user.Email == Email);
            return user == null;
        }


        public async Task<LoginResponseDTO> Login(LoginRequestDTO Request)
        {
            var users = _db.LocalUsers.ToList();
            var user = _db.LocalUsers.FirstOrDefault(user => user.Email.ToLower() == Request.Email.ToLower() && Request.Password == user.Password);
            if (user == null)
            {
                return null;
            }


            //get token if user found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.ID.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponse = new()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };
            return loginResponse;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO Request)
        {
            LocalUser user = new LocalUser()
            {
            Email = Request.Email,
            Password = Request.Password,
            Name = Request.Name,
            Role = Request.Role,
            };
            await _db.LocalUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

    }
}
