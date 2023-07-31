using MagicVillaApi.Modelos;
using MagicVillaApi.Modelos.Dto;
using MagicVillaApi.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using AutoMapper;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger = null, ApplicationDbContext db = null, IMapper mapper = null)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Villas recuperadas con exito!");

            //return Ok(VillaStore.villaList);
            //return Ok(await _db.Villas.ToListAsync());

            // creamos la lista general sin mapearla
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            // devolvemos la lista mapeada
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));


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
        // Se utiliza ActionResult cuando devolvemos una respuesta
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al recuperar la villa con id: " + id);
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //return Ok(villa);
            // modelo mapeado
            return Ok(_mapper.Map<VillaDto>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<VillaDto>> AddVilla([FromBody] VillaCreateDto createDto)
        {
            // validaciones con DataAnnotations
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // validacion personalizadas
            //if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)

            {
                ModelState.AddModelError("NombreExiste", "El  nombre de la villa ya existe");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);
            }

            //if (villaDto.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError);
            //}

            //villaDto.Id = _db.Villas.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villaDto);

            // creacion de modelo nuevo sin mapear
            //Villa modelo = new Villa
            //{
            //    //Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    Tarifa = villaDto.Tarifa,
            //    Ocupantes = villaDto.Ocupantes,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    ImageUrl = villaDto.ImageUrl,
            //    Amenidad = villaDto.Amenidad
            //};

            // modelo simplificado
            Villa modelo = _mapper.Map<Villa>(createDto);

            //
            // Remove y Update no son metodo asyncronos
            //

            await _db.Villas.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);
            //return Ok(villaDto);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // Se utiliza IActionResult cuando devolvemos NoContent como respuesta
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //VillaStore.villaList.Remove(villa);
            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {

            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //var villa = _db.Villas.FirstOrDefault(v => v.Id == id)

            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    Tarifa = villaDto.Tarifa,
            //    //Ocupantes = villaDto.Ocupantes,
            //    //MetrosCuadrados = villaDto.MetrosCuadrados,
            //    //ImageUrl = villaDto.ImageUrl,
            //    //Amenidad = villaDto.Amenidad,
            //    FechaActualizacion = DateTime.Now
            //};

            Villa modelo = _mapper.Map<Villa>(updateDto);

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {

            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id); // estos es importante para que no  genere un error ni duplicado

            if (villa == null) { return BadRequest(); }

            //VillaUpdateDto villaDto = new()
            //{
            //    Id = villa.Id,
            //    Nombre = villa.Nombre,
            //    Detalle = villa.Detalle,
            //    Tarifa = villa.Tarifa,
            //    //Ocupantes = villa.Ocupantes,
            //    //MetrosCuadrados = villa.MetrosCuadrados,
            //    //ImageUrl = villa.ImageUrl,
            //    //Amenidad = villa.Amenidad,
            //};

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    Tarifa = villaDto.Tarifa,
            //    //Ocupantes = villaDto.Ocupantes,
            //    //MetrosCuadrados = villaDto.MetrosCuadrados,
            //    //ImageUrl = villaDto.ImageUrl,
            //    //Amenidad = villaDto.Amenidad,
            //    FechaActualizacion = DateTime.Now
            //};

            Villa modelo = _mapper.Map<Villa>(villaDto);

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
