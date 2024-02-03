using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Repositories.IRepositories;

namespace MagicVilla_API.Repositories
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly VillaDbContext _db;

        public VillaRepository(VillaDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.Villas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
