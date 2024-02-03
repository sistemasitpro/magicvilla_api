using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Repositories.IRepositories
{
    public interface IUsuarioRepository
    {
        bool IsUsuarioUnico(string username);
        Task<LoginResponseDto> Login(LoginRequestDto dto);
        Task<UsuarioDto> Register(RegisterRequestDto dto);
    }
}
