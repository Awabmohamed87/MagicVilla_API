using AutoMapper;
using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper) : base(db)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string Email)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(user => user.Email == Email);
            return user == null;
        }


        public async Task<LoginResponseDTO> Login(LoginRequestDTO Request)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(user => user.Email.ToLower() == Request.Email.ToLower());
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, Request.Password); 
            if (user == null || !isPasswordValid)
            {
                return null;
            }


            //get token if user found
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var roles = await _userManager.GetRolesAsync(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                   
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponse = new()
            {
                Token = tokenHandler.WriteToken(token),
                User =  _mapper.Map<UserDTO>(user)
            };
            return loginResponse;
        }

        public async Task<UserDTO> Register(RegistrationRequestDTO Request)
        {
            ApplicationUser user = new ApplicationUser()
            {
            Email = Request.Email,
            NormalizedEmail = Request.Email.ToUpper(),
            Name = Request.Name,
            UserName = Request.Email
            };
            try
            {
                var result = await _userManager.CreateAsync(user, Request.Password);
                if(result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));
                        await _roleManager.CreateAsync(new IdentityRole("Customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "Admin");
                    ApplicationUser resultUser = await _db.ApplicationUsers.FirstOrDefaultAsync(user => user.Email == Request.Email);
                    return _mapper.Map<UserDTO>(resultUser);
                }
            }
            catch(Exception ex)
            {

            }
            return new UserDTO();
        }

    }
}
