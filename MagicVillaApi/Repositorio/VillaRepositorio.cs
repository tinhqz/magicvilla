using AutoMapper.Execution;
using MagicVillaApi.Datos;
using MagicVillaApi.Modelos;
using MagicVillaApi.Repositorio.IRepositorio;

namespace MagicVillaApi.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _dbctx;

        public VillaRepositorio(ApplicationDbContext dbctx) : base(dbctx)
        {
            _dbctx = dbctx;
        }

        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _dbctx.Villas.Update(entidad);
            await _dbctx.SaveChangesAsync();
            return entidad;
        }
    }
}
