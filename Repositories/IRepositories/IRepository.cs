using MagicVilla_API.Models.Especificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MagicVilla_API.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task Crear(T entidad);
        Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null);
        PagedList<T> ObtenerTodosPaginados(Parametros parametros, Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null);
        Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true, string? incluirPropiedades = null);
        Task Remover(T entidad);
        Task Grabar();

    }
}
