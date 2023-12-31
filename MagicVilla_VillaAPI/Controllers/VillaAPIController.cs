using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/VillaAPI")]
    //[Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepository _repo;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _response = new APIResponse();
        }

        #region Get

        [HttpGet(Name = "GetVillas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillasAsync()
        {
            try
            {
                IEnumerable<Villa> villas = await _repo.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villas);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[ProducesResponseType(200, Type = typeof(VillaDTO))]*/
        public async Task<ActionResult<APIResponse>> GetVillaAsync(int id)
        {
            try { 
                if (id == 0)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
            
                Villa villa = await _repo.GetFirstOrDefaultAsync(villa => villa.Id == id);

                if (villa == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }
            return _response;
        }
        #endregion

        #region Post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> AddVillaAsync([FromBody] VillaCreateDTO villaCreateDTO)
        {
            //Validate model if [ApiController] isn't there
            //if(!ModelState.IsValid) 
            //    return BadRequest(ModelState);
            try { 

                if (await _repo.GetFirstOrDefaultAsync(villa => villa.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa Already Exists!");
                    return BadRequest(ModelState);
                }

                if (villaCreateDTO == null)
                {
                    return BadRequest();
                }
                Villa villaToCreate = _mapper.Map<Villa>(villaCreateDTO);
                
                await _repo.CreateAsync(villaToCreate);

                _response.Result = _mapper.Map<VillaDTO>(villaToCreate);
                _response.StatusCode = System.Net.HttpStatusCode.Created;
            

                return CreatedAtRoute("GetVilla", new { id = villaToCreate.Id }, _response);
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }
            return _response;
        }
        #endregion

        #region Delete
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaAsync(int id)
        {
            try { 
                if (id == 0)
                    return BadRequest();

                Villa villaToDelete = await _repo.GetFirstOrDefaultAsync(villa => villa.Id == id);
                if (villaToDelete == null)
                {
                    return NotFound();
                }
                await _repo.Remove(villaToDelete);

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
        #endregion

        #region Put
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int id, [FromBody]VillaUpdateDTO villaUpdateDTO)
        {
            try { 
                if(villaUpdateDTO == null || id != villaUpdateDTO.Id)
                    return BadRequest();

                Villa villaToUpdate = _mapper.Map<Villa>(villaUpdateDTO);

                await _repo.Update(villaToUpdate);

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
        #endregion

        #region patch
        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaUpdateDTO> patch)
        {
            if(id == 0 || patch == null)
                return BadRequest();

            Villa villaToUpdate = await _repo.GetFirstOrDefaultAsync(villa => villa.Id == id,isTracking : false);
            if (villaToUpdate == null)
                return NotFound();

            VillaUpdateDTO villadto = _mapper.Map<VillaUpdateDTO>(villaToUpdate);
            patch.ApplyTo(villadto, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.Update(_mapper.Map<Villa>(villadto));

            return NoContent();
        }
        #endregion
    }
}
