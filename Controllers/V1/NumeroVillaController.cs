using AutoMapper;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MagicVilla_API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepository _villaRepo;
        private readonly INumeroVillaRepository _numerovillaRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepository villaRepo, INumeroVillaRepository numerovillaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numerovillaRepo = numerovillaRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetNumeroVillas()
        {
            try
            {
                IEnumerable<NumeroVilla> numeroVillaList = await _numerovillaRepo.ObtenerTodos(incluirPropiedades: "Villa");
                var mapper = _mapper.Map<IEnumerable<NumeroVillaResponse>>(numeroVillaList);
                _logger.LogInformation("Se obtuvieron todas las villas");
                _response.Data = mapper;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);

            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetNumeroVillaPorId(Guid id)
        {
            try
            {
                var numeroVilla = await _numerovillaRepo.Obtener(v => v.Id.Equals(id), true, incluirPropiedades: "Villa");

                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "Numero Villa no encontrada, no hubieron coincidencias" };
                    return NotFound(_response);
                }

                var mapper = _mapper.Map<NumeroVillaResponse>(numeroVilla);

                _response.Data = mapper;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpGet("nro/{villanro}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> GetNumeroVillaPorVillaNro(int villanro)
        {
            try
            {
                var numeroVilla = await _numerovillaRepo.Obtener(v => v.VillaNro == villanro, true);

                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "Numero Villa no encontrada, no hubieron coincidencias" };
                    return NotFound(_response);
                }

                var mapper = _mapper.Map<NumeroVillaResponse>(numeroVilla);

                _response.Data = mapper;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ApiResponse>> CreateNumeroVilla([FromBody] NumeroVillaCreateRequest dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _numerovillaRepo.Obtener(v => v.VillaNro == dto.VillaNro, false) != null)
                {
                    ModelState.AddModelError("ErrorMensaje", "El numero de Villa  ya existe!");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.Obtener(v => v.Id.Equals(dto.VillaId), false) == null)
                {
                    ModelState.AddModelError("ErrorMensaje", "El Id de la Villa  no existe!");
                    return BadRequest(ModelState);
                }

                if (dto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "No se ha enviado un numero villa" };
                    return BadRequest(_response);
                }

                NumeroVilla numeroVilla = _mapper.Map<NumeroVilla>(dto);
                numeroVilla.FechaCreacion = DateTime.Now;
                numeroVilla.FechaActualizacion = DateTime.Now;

                await _numerovillaRepo.Crear(numeroVilla);

                _response.Data = "Numero Villa creada con éxito.";
                _response.StatusCode = HttpStatusCode.Created;

                return StatusCode(StatusCodes.Status201Created, _response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateNumeroVilla(Guid id, [FromBody] NumeroVillaUpdateRequest dto)
        {
            try
            {
                if (dto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "No has enviado datos" };
                    return BadRequest(_response);
                }

                var numeroVilla = await _numerovillaRepo.Obtener(nv => nv.Id.Equals(id), false);

                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "Numero Villa no encontrada, no hubieron coincidencias" };
                    return NotFound(_response);
                }

                if (await _villaRepo.Obtener(v => v.Id.Equals(dto.VillaId), false) == null)
                {
                    ModelState.AddModelError("ErrorMensaje", "El Id de la Villa No existe!");
                    return BadRequest(ModelState);
                }

                dto.Id = id;
                NumeroVilla modelo = _mapper.Map<NumeroVilla>(dto);

                modelo.FechaCreacion = numeroVilla.FechaCreacion;
                modelo.FechaActualizacion = DateTime.Now;

                await _numerovillaRepo.Actualizar(modelo);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = "Numero Villa actualizada con exito";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdatePartialNumeroVilla(Guid id, JsonPatchDocument<NumeroVillaUpdateRequest> patchDocument)
        {
            try
            {
                var numeroVilla = await _numerovillaRepo.Obtener(v => v.Id.Equals(id), false);

                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "Numero Villa no encontrada, no hubieron coincidencias" };
                    return NotFound(_response);
                }


                NumeroVillaUpdateRequest numeroVillaDto = _mapper.Map<NumeroVillaUpdateRequest>(numeroVilla);

                patchDocument.ApplyTo(numeroVillaDto, ModelState);

                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.Data = ModelState;
                    return BadRequest(_response);
                }

                if (await _villaRepo.Obtener(v => v.Id.Equals(numeroVillaDto.VillaId), false) == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "EL id de la villa no existe" };
                    return BadRequest(_response);
                }


                NumeroVilla modelo = _mapper.Map<NumeroVilla>(numeroVillaDto);

                modelo.FechaCreacion = numeroVilla.FechaCreacion;
                modelo.FechaActualizacion = DateTime.Now;

                await _numerovillaRepo.Actualizar(modelo);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = "Numero Villa actualizada con exito";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

        [HttpDelete("{VillaNro}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteNumeroVilla(int VillaNro)
        {
            try
            {
                var numeroVilla = await _numerovillaRepo.Obtener(nv => nv.VillaNro == VillaNro, true);

                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMensaje = new List<string>() { "Numero Villa no encontrada, no hubieron coincidencias" };
                    return NotFound(_response);
                }

                await _numerovillaRepo.Remover(numeroVilla);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Data = "Numero Villa borrada con éxito";

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }
    }
}
