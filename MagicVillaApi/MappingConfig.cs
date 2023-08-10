using AutoMapper;
using MagicVillaApi.Modelos;
using MagicVillaApi.Modelos.Dto;

namespace MagicVillaApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            // Crear el mapping individual por cada modelo, metodo largo: 
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();

            // Metodo abreviado
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();
        }
    }
}
