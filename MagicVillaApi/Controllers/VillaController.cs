using MagicVillaApi.Modelos;
using MagicVillaApi.Modelos.Dto;
using MagicVillaApi.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {

            return Ok(VillaStore.villaList);

            // metodo simple 
            //
            //return new List<VillaDto> {
            //    new VillaDto { Id = 1, Nombre="Vista a la playa"},
            //    new VillaDto { Id = 1, Nombre="Vista a la piscina"}
            //};
        }

        //[HttpGet("id")]
        //https://localhost:7076/api/Villa/id?id=1


        [HttpGet("{id}", Name = "GetVilla")]
        //https://localhost:7076/api/Villa/1
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<VillaDto> AddVilla([FromBody] VillaDto villaDto)
        {
            // validaciones con DataAnnotations
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            // validacion personalizadas
            if (VillaStore.villaList.FirstOrDefault(v=>v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null) {
                ModelState.AddModelError("NombreExiste" ,"El  nombre de la villa ya existe");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }

            if (villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
            //return Ok(villaDto);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult DeleteVilla(int id) {
            if (id ==0) {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v=>v.Id == id);

            if (villa == null) {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);

            return NoContent();            
        }
    }
}
