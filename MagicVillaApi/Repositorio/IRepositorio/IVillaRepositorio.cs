﻿using MagicVillaApi.Modelos;

namespace MagicVillaApi.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio <Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
