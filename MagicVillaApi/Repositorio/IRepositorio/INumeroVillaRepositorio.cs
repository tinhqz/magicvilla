using MagicVillaApi.Modelos;

namespace MagicVillaApi.Repositorio.IRepositorio
{
    public interface INumeroVillaRepositorio : IRepositorio <NumeroVilla>
    {
        Task<NumeroVilla> Actualizar(NumeroVilla entidad);
    }
}
