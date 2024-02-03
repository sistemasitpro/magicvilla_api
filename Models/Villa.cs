using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = new Guid();
        [Required(ErrorMessage = "El Nombre es requerido")]
        [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres")]
        public String Nombre { get; set; } = "";
        [Required(ErrorMessage = "El Detalle es requerido")]
        [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres")]
        public String Detalle { get; set; } = "";
        [Required(ErrorMessage = "La tarifa es requerida")]
        public Double Tarifa { get; set; } = 0.0;
        public int Ocupantes { get; set; }
        public int MetrosCuadrados { get; set; }
        [MaxLength(255, ErrorMessage = "Maximo de 255 caracteres")]

        public String? ImagenUrl { get; set; } = "";
        [MaxLength(100, ErrorMessage = "Maximo de 100 caracteres")]

        public String? Amenidad { get; set; } = "";
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaActualizacion { get; set; }
    }
}
