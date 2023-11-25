using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/UserAPI")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserAPIController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginRequestDTO User)
        {
            APIResponse APIResponse;
            var Users = await _userRepository.Login(User);
            if(Users == null || string.IsNullOrEmpty(Users.Token))
            {
                APIResponse = new()
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>
                    {
                        "User or Password is incorrect!!"
                    }
                };
                return BadRequest(APIResponse);
            }
            APIResponse = new (){
                Result = _mapper.Map<LoginResponseDTO>(Users),
                StatusCode = System.Net.HttpStatusCode.OK
            };
            return Ok(APIResponse);
        }
       
        [HttpPost("Register")]
        public async Task<ActionResult<APIResponse>> AddUser([FromBody] RegistrationRequestDTO User)
        {
            APIResponse response;
            if (!_userRepository.IsUniqueUser(User.Email))
            {
                response = new()
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>
                    {
                        "User Already Exists !!"
                    }
                };
                return BadRequest(response);
            }
            var  user = await _userRepository.Register(User);
            if(user == null)
            {
                response = new()
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = new List<string>
                    {
                        "User Already Exists !!"
                    }
                };
                return BadRequest(response);
            }
            response = new()
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK
            };
            return Ok(response);
        }
    }
}
