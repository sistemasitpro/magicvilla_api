using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepositories;

namespace MagicVilla_API.Repositories
{
    public class NumeroVillaRepository : Repository<NumeroVilla>, INumeroVillaRepository
    {
        private readonly VillaDbContext _db;

        public NumeroVillaRepository(VillaDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroVillas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
