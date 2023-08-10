using MagicVillaApi.Modelos;
using MagicVillaApi.Modelos.Dto;
using MagicVillaApi.Datos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using AutoMapper;
using MagicVillaApi.Repositorio.IRepositorio;
using System.Net;

namespace MagicVillaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        //private readonly ApplicationDbContext _db; Se sustituye por VillaRepositorio
        private readonly IVillaRepositorio _villaRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        // ApplicationDbContext db = null
        public VillaController(ILogger<VillaController> logger = null, IVillaRepositorio villaRepositorio = null, IMapper mapper = null)
        {
            _logger = logger;
            //_db = db;
            _villaRepositorio = villaRepositorio;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Villas recuperadas con exito!");

                //return Ok(VillaStore.villaList);
                //return Ok(await _db.Villas.ToListAsync());

                // creamos la lista general sin mapearla
                // IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

                IEnumerable<Villa> villaList = await _villaRepositorio.ObtenerTodos();
                // devolvemos la lista mapeada

                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
                _response.statusCode = HttpStatusCode.OK;

                return Ok(_response);


                // metodo simple 
                //
                //return new List<VillaDto> {
                //    new VillaDto { Id = 1, Nombre="Vista a la playa"},
                //    new VillaDto { Id = 1, Nombre="Vista a la piscina"}
                //};
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
            
        }

        //[HttpGet("id")]
        //https://localhost:7076/api/Villa/id?id=1


        [HttpGet("{id}", Name = "GetVilla")]
        //https://localhost:7076/api/Villa/1
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // Se utiliza ActionResult cuando devolvemos una respuesta
        //public async Task<ActionResult<VillaDto>> GetVilla(int id)
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al recuperar la villa con id: " + id);
                   _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

                //var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

                var villa = await _villaRepositorio.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<VillaDto>(villa);
                _response.statusCode = HttpStatusCode.OK;

                //return Ok(villa);
                // modelo mapeado
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //public async Task<ActionResult<VillaDto>> AddVilla([FromBody] VillaCreateDto createDto)
        public async Task<ActionResult<APIResponse>> AddVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                // validaciones con DataAnnotations
                if (!ModelState.IsValid)
                {
                    // pendiente por intentar mejora
                    return BadRequest(ModelState);
                }

                // validacion personalizadas
                //if (VillaStore.villaList.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
                //if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                if (await _villaRepositorio.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    // pendiente por intentar mejora
                    ModelState.AddModelError("NombreExiste", "El  nombre de la villa ya existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    // pendiente por intentar mejora
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

                //await _db.Villas.AddAsync(modelo);
                //await _db.SaveChangesAsync();

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _villaRepositorio.Crear(modelo);

                _response.Resultado = modelo;
                _response.statusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
                //return Ok(villaDto);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        // Se utiliza IActionResult cuando devolvemos NoContent como respuesta
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                //var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
                var villa = await _villaRepositorio.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //VillaStore.villaList.Remove(villa);
                //_db.Villas.Remove(villa);
                //await _db.SaveChangesAsync();
               await _villaRepositorio.Remover(villa);
                _response.statusCode = HttpStatusCode.NoContent;

                //return NoContent();
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            // Con la interfaz IActionResult no se puede devolver otro tipo de interfaz
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
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

                //_db.Villas.Update(modelo);
                //await _db.SaveChangesAsync();
               await _villaRepositorio.Actualizar(modelo);
                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            try
            {
                // { "op": "replace", "path": "/nombre", "value": "boo" },

                if (patchDto == null || id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                //var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id); // estos es importante para que no  genere un error ni duplicado
                var villa = await _villaRepositorio.Obtener(v => v.Id == id, tracked: false); // estos es importante para que no  genere un error ni duplicado

                if (villa == null) {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

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
                    // pendiente por mejorar
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

                //_db.Villas.Update(modelo);
                //await _db.SaveChangesAsync();
                await _villaRepositorio.Actualizar(modelo);
                _response.statusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }
    }
}
