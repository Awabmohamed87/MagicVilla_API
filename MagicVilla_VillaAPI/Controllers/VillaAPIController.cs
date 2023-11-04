using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    //[Route("api/[controller]")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        #region Get
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas() => Ok(VillaStore.VillasList);

        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        /*[ProducesResponseType(200, Type = typeof(VillaDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]*/
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
                return BadRequest();
            VillaDTO villa = VillaStore.VillasList.FirstOrDefault(villa => villa.Id == id);
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

            if (VillaStore.VillasList.FirstOrDefault(villa => villa.Name.ToLower() == villaDTO.Name.ToLower()) != null)
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
            int maxId = VillaStore.VillasList.MaxBy(villa => villa.Id).Id;
            villaDTO.Id = maxId + 1;
            VillaStore.VillasList.Add(villaDTO);
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

            VillaDTO villaToDelete = VillaStore.VillasList.FirstOrDefault(villa => villa.Id == id);
            if (villaToDelete == null)
            {
                return NotFound();
            }
            VillaStore.VillasList.Remove(villaToDelete);

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

            VillaDTO villaToUpdate = VillaStore.VillasList.FirstOrDefault(villa=> villa.Id == id);

            if(villaToUpdate == null)
                return NotFound();

            villaToUpdate.Name = villa.Name;    
            villaToUpdate.Occupancy = villa.Occupancy;
            villaToUpdate.sqft = villa.sqft;
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

            VillaDTO villaToUpdate = VillaStore.VillasList.FirstOrDefault(villa => villa.Id == id);
            if (villaToUpdate == null)
                return NotFound();

            patch.ApplyTo(villaToUpdate, ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }
        #endregion
    }
}
