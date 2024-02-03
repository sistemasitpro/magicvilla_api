using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.Dto
{
    public class NumeroVillaCreateRequest
    {
        [Required(ErrorMessage = "Tienes que ingresar un numero de villa")]
        public int VillaNro { get; set; }
        [Required(ErrorMessage = "El id de la villa es requerido")]
        public Guid VillaId { get; set; }
        public String DetalleEspecial { get; set; } = "";
    }
}
