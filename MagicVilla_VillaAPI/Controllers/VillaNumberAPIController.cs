using Asp.Versioning;
using AutoMapper;
using Azure;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _response;
        public VillaNumberAPIController(IVillaNumberRepository villaNumberRepository, IVillaRepository villaRepository, IMapper mapper)
        {
            _villaNumberRepository = villaNumberRepository;
            _villaRepository = villaRepository;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public async Task<ActionResult<APIResponse>> GetVillasNumbers()
        {
            try { 
                IEnumerable<VillaNumber> villas = await _villaNumberRepository.GetAllAsync(includeProperties: "Villa"); 
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villas);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString()};
            }
            return _response;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[]{"v1", "v2"};
        }

        [HttpGet("{villaNumber:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int villaNumber)
        {
            try { 
                if(villaNumber == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                VillaNumber retrievedVilla = await _villaNumberRepository.GetFirstOrDefaultAsync(e => e.VillaNo == villaNumber, includeProperties:"Villa");
                if(retrievedVilla == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.IsSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(retrievedVilla);
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString()};
            }
            return _response;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaToCreate)
        {
            if(villaToCreate.VillaNo == 0 || await _villaNumberRepository.GetFirstOrDefaultAsync(e => e.VillaNo == villaToCreate.VillaNo) != null)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                ModelState.AddModelError("ErrorMessages", "Villa Number Already Exists!!");
                return BadRequest(ModelState);
            }

            if (await _villaRepository.GetFirstOrDefaultAsync(e => e.Id == villaToCreate.VillaId) == null)
            {
                ModelState.AddModelError("ErrorMessages", "Villa With Id " + villaToCreate.VillaId.ToString() + " Doesn't exist!!");
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(ModelState);
            }

            VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaToCreate);
            await _villaNumberRepository.CreateAsync(villaNumber);
            _response.IsSuccess = true;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            return CreatedAtRoute("GetVillaNumber", new {villaNumber = villaToCreate.VillaNo}, _response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{villaNumber:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaAsync(int villaNumber)
        {
            try
            {
                if (villaNumber == 0)
                    return BadRequest();

                VillaNumber villaToDelete = await _villaNumberRepository.GetFirstOrDefaultAsync(villa => villa.VillaNo == villaNumber);
                if (villaToDelete == null)
                {
                    return NotFound();
                }
                await _villaNumberRepository.Remove(villaToDelete);

                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;

                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{villaNumber:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int villaNumber, [FromBody] VillaNumberUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || villaNumber != villaUpdateDTO.VillaNo)
                    return BadRequest();

                VillaNumber villaToUpdate = _mapper.Map<VillaNumber>(villaUpdateDTO);

                await _villaNumberRepository.UpdateAsync(villaToUpdate);

                _response.StatusCode = System.Net.HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }
            return _response;
        }

    }
}
