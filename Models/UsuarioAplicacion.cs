using Microsoft.AspNetCore.Identity;

namespace MagicVilla_API.Models
{
    public class UsuarioAplicacion: IdentityUser
    {
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
    }
}
