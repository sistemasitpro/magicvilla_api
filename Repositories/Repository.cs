using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using MagicVilla_API.Data;
using MagicVilla_API.Models.Especificaciones;
using MagicVilla_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VillaDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(VillaDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Crear(T entidad)
        {
            await dbSet.AddAsync(entidad);
            await Grabar();
        }

        public async Task Grabar()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true, string? incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if(incluirPropiedades!= null)
            {
                foreach(var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            var response = await query.FirstOrDefaultAsync();

            return response ?? default!;
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            return await query.ToListAsync();
        }

        public PagedList<T> ObtenerTodosPaginados(Parametros parametros, Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
        }

        public async Task Remover(T entidad)
        {
            dbSet.Remove(entidad);
            await Grabar();
        }
    }
}
