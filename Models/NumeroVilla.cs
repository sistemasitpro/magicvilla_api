using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models
{
    public class NumeroVilla
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = new Guid();
        [Required(ErrorMessage = "Tienes que ingresar un numero de villa")]
        public int VillaNro { get; set; }
        [Required(ErrorMessage = "El id de la villa es requerido")]
        public Guid VillaId { get; set; }
        [ForeignKey("VillaId")]
        public Villa? Villa { get; set; }
        public String DetalleEspecial { get; set; } = "";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
    }
}
