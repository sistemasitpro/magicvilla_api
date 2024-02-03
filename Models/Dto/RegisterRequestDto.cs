namespace MagicVilla_API.Models.Dto
{
    public class RegisterRequestDto
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
