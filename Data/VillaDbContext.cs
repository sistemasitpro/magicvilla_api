using MagicVilla_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Data
{
    public class VillaDbContext : IdentityDbContext<UsuarioAplicacion>
    {

        public VillaDbContext(DbContextOptions<VillaDbContext> options) : base(options)
        {
        }

        public DbSet<UsuarioAplicacion> UsuariosAplicacion { get; set; }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVilla> NumeroVillas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guid nuevoGuid = Guid.NewGuid();
            Guid nuevoGuid2 = Guid.NewGuid();
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = nuevoGuid,
                    Nombre = "Villa real",
                    Detalle = "Detalle de la villa",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 10,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa
                {
                    Id = nuevoGuid2,
                    Nombre = "Premium vista a la piscina",
                    Detalle = "Detalle de la villa",
                    ImagenUrl = "",
                    Ocupantes = 4,
                    MetrosCuadrados = 40,
                    Tarifa = 250,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }
            );
        }
    }
}
