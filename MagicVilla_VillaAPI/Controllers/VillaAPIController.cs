using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Data.ApplicationDbContext;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    //[Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaAPIController(ApplicationDbContext db, IMapper mapper)
        {
              _db = db;
            _mapper = mapper;
        }

        #region Get

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillasAsync()
        {
            IEnumerable<Villa> villas = await _db.Villas_Api.ToListAsync();
            return Ok(_mapper.Map<VillaDTO>(villas));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[ProducesResponseType(200, Type = typeof(VillaDTO))]*/
        public async Task<ActionResult<VillaDTO>> GetVillaAsync(int id)
        {
            if (id == 0)
                return BadRequest();
            
            Villa villa = await _db.Villas_Api.FirstOrDefaultAsync(villa => villa.Id == id);
            if (villa == null)
                return NotFound();
            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        #endregion

        #region Post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> AddVillaAsync([FromBody] VillaCreateDTO villaCreateDTO)
        {
            //Validate model if [ApiController] isn't there
            //if(!ModelState.IsValid) 
            //    return BadRequest(ModelState);

            if (await _db.Villas_Api.FirstOrDefaultAsync(villa => villa.Name.ToLower() == villaCreateDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Villa Already Exists!");
                return BadRequest(ModelState);
            }

            if (villaCreateDTO == null)
            {
                return BadRequest();
            }
            Villa villaToCreate = _mapper.Map<Villa>(villaCreateDTO);
            
            await _db.Villas_Api.AddAsync(villaToCreate);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = villaToCreate.Id }, villaToCreate);
        }
        #endregion

        #region Delete
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletVillaAsync(int id)
        {
            if (id == 0)
                return BadRequest();

            Villa villaToDelete = await _db.Villas_Api.FirstOrDefaultAsync(villa => villa.Id == id);
            if (villaToDelete == null)
            {
                return NotFound();
            }
            _db.Villas_Api.Remove(villaToDelete);
            await _db.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Put
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVillaAsync(int id, [FromBody]VillaUpdateDTO villaUpdateDTO)
        {
            if(villaUpdateDTO == null || id != villaUpdateDTO.Id)
                return BadRequest();

            Villa villaToUpdate = _mapper.Map<Villa>(villaUpdateDTO);

            _db.Villas_Api.Update(villaToUpdate);
            await _db.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region patch
        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaUpdateDTO> patch)
        {
            if(id == 0 || patch == null)
                return BadRequest();

            Villa villaToUpdate = await _db.Villas_Api.AsNoTracking().FirstOrDefaultAsync(villa => villa.Id == id);
            if (villaToUpdate == null)
                return NotFound();

            VillaUpdateDTO villadto = _mapper.Map<VillaUpdateDTO>(villaToUpdate);
            patch.ApplyTo(villadto, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Villas_Api.Update(_mapper.Map<Villa>(villadto));
            await _db.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}
