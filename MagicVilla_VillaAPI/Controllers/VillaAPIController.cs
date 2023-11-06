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
        public VillaAPIController(ApplicationDbContext db)
        {
              _db = db;
        }
        #region Get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas() => Ok(_db.Villas_Api);
        

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[ProducesResponseType(200, Type = typeof(VillaDTO))]*/
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
                return BadRequest();
            
            Villa villa = _db.Villas_Api.FirstOrDefault(villa => villa.Id == id);
            if (villa == null)
                return NotFound();
            return Ok(villa);
        }
        #endregion

        #region Post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> AddVilla([FromBody] VillaDTO villaDTO)
        {
            //Validate model if [ApiController] isn't there
            //if(!ModelState.IsValid) 
            //    return BadRequest(ModelState);

            if (_db.Villas_Api.FirstOrDefault(villa => villa.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("", "Villa Already Exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest();
            }
            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            _db.Villas_Api.Add(new Villa { 
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                sqft = villaDTO.sqft,
                Occupancy = villaDTO.Occupancy,
                Amenity = villaDTO.Amenity,
                ImageUrl = villaDTO.ImageUrl,
                Rate = villaDTO.Rate
            });
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }
        #endregion

        #region Delete
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            Villa villaToDelete = _db.Villas_Api.FirstOrDefault(villa => villa.Id == id);
            if (villaToDelete == null)
            {
                return NotFound();
            }
            _db.Villas_Api.Remove(villaToDelete);
            _db.SaveChanges();

            return NoContent();
        }
        #endregion

        #region Put
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villa)
        {
            if(villa == null || id != villa.Id)
                return BadRequest();

            Villa villaToUpdate = _db.Villas_Api.FirstOrDefault(villa=> villa.Id == id);

            if(villaToUpdate == null)
                return NotFound();

            villaToUpdate.Name = villa.Name;    
            villaToUpdate.Occupancy = villa.Occupancy;
            villaToUpdate.sqft = villa.sqft;

            _db.Villas_Api.Update(villaToUpdate);
            _db.SaveChanges();
            //int index = VillaStore.VillasList.IndexOf(villaToUpdate);
            //VillaStore.VillasList[index] = villa;
            return NoContent();
        }
        #endregion

        #region patch
        [HttpPatch("{id:int}",Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patch)
        {
            if(id == 0 || patch == null)
                return BadRequest();

            Villa villaToUpdate = _db.Villas_Api.AsNoTracking().FirstOrDefault(villa => villa.Id == id);
            if (villaToUpdate == null)
                return NotFound();

            VillaDTO villadto = new VillaDTO
            {
                Id = villaToUpdate.Id,
                Name = villaToUpdate.Name,
                Occupancy = villaToUpdate.Occupancy,
                sqft = villaToUpdate.sqft,
                Details = villaToUpdate.Details,
                ImageUrl = villaToUpdate.ImageUrl,
                Amenity = villaToUpdate.Amenity,
                Rate = villaToUpdate.Rate


            };
            patch.ApplyTo(villadto, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Villas_Api.Update(new Villa
            {
                Id = villadto.Id,
                Name = villadto.Name,
                Occupancy = villadto.Occupancy,
                sqft = villadto.sqft,
                Details = villadto.Details,
                ImageUrl = villadto.ImageUrl,
                Amenity = villadto.Amenity,
                Rate = villadto.Rate
            });
            _db.SaveChanges();

            return NoContent();
        }
        #endregion
    }
}
