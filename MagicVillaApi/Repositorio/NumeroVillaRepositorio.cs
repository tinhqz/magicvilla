using AutoMapper.Execution;
using MagicVillaApi.Datos;
using MagicVillaApi.Modelos;
using MagicVillaApi.Repositorio.IRepositorio;

namespace MagicVillaApi.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly ApplicationDbContext _dbctx;

        public NumeroVillaRepositorio(ApplicationDbContext dbctx) : base(dbctx)
        {
            _dbctx = dbctx;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _dbctx.NumeroVillas.Update(entidad);
            await _dbctx.SaveChangesAsync();
            return entidad;
        }
    }
}
