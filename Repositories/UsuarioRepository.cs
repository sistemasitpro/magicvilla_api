using AutoMapper;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly VillaDbContext _db;
        private string secretKey;
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UsuarioRepository(VillaDbContext db, IConfiguration configuration, UserManager<UsuarioAplicacion> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public bool IsUsuarioUnico(string username)
        {
            var usuario = _db.UsuariosAplicacion.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());

            if (usuario != null)
            {
                return false;
            }

            return true;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var usuario = await _db.UsuariosAplicacion.FirstOrDefaultAsync(u => u.UserName.ToLower() == dto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuario, dto.Password);
           
            if (usuario == null || !isValid)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            var roles = await _userManager.GetRolesAsync(usuario);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires= DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto response = new()
            {
                Token = tokenHandler.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDto>(usuario)
            };

            return response;
        }

        public async Task<UsuarioDto> Register(RegisterRequestDto dto)
        {
            UsuarioAplicacion usuario = new();

            usuario.UserName = dto.UserName;
            usuario.NormalizedUserName = dto.UserName.ToUpper();
            usuario.Nombre = dto.Nombre;
            usuario.Apellido= dto.Apellido;
            usuario.Email = dto.Email;
            usuario.NormalizedEmail = dto.Email.ToUpper();

            try
            {
                var resultado = await _userManager.CreateAsync(usuario, dto.Password);

                if(resultado.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                    }

                    await _userManager.AddToRoleAsync(usuario, dto.Rol);
                    var usuarioAp = _db.UsuariosAplicacion.FirstOrDefault(u => u.UserName == dto.UserName);
                    return _mapper.Map<UsuarioDto>(usuarioAp);
                }
            } catch(Exception ex)
            {
                throw;
            }

            return null;
        }
    }
}
