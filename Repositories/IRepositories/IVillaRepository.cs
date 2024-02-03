using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_API.Models;

namespace MagicVilla_API.Repositories.IRepositories
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
