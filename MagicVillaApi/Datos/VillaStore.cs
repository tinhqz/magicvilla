﻿using MagicVillaApi.Modelos.Dto;

namespace MagicVillaApi.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
                new VillaDto { Id = 1, Nombre="Vista a la playa"},
                new VillaDto { Id = 2, Nombre="Vista a la piscina"}
        };
    }
}
