using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [ApiVersionNeutral]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private ApiResponse _response;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _response = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var loginResponse = await _usuarioRepository.Login(dto);

            if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMensaje.Add("El usuario y/o la contraseña son incorrectos");
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = loginResponse;

            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            bool isUsuarioUnico = _usuarioRepository.IsUsuarioUnico(dto.UserName);

            if (!isUsuarioUnico)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMensaje.Add("El usuario ya existe en nuestra base de datos");
                return BadRequest(_response);
            }

            var usuario = await _usuarioRepository.Register(dto);

            if (usuario == null)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMensaje.Add("Error al registrar el usuario");
                return BadRequest(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            _response.Data = usuario;

            return Ok(_response);
        }
    }
}
