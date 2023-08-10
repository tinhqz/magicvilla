using MagicVillaApi.Datos;
using MagicVillaApi.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MagicVillaApi.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _dbctx;
        internal DbSet<T> _dbSet;

        public Repositorio(ApplicationDbContext dbctx) {
            _dbctx = dbctx;
            this._dbSet = _dbctx.Set<T>();
        }

        public async Task Crear(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            await Grabar();
        }

        public async Task Grabar()
        {
            await _dbctx.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            { 
                query = query.AsNoTracking();
            }
            if (filtro != null)
            { 
                query = query.Where(filtro);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = _dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }

        public async Task Remover(T entidad)
        {
            _dbSet.Remove(entidad);
            await Grabar();
        }
    }
}
