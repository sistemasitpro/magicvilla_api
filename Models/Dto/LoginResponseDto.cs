namespace MagicVilla_API.Models.Dto
{
    public class LoginResponseDto
    {
        public UsuarioDto Usuario { get; set; }
        public string Token { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
