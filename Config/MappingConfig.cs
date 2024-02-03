using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Config
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
           CreateMap<Villa, VillaDto>().ReverseMap();

            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaResponse>().ReverseMap();

            CreateMap<NumeroVilla, NumeroVillaCreateRequest>().ReverseMap();
            CreateMap<NumeroVilla, NumeroVillaUpdateRequest>().ReverseMap();

            CreateMap<UsuarioAplicacion, UsuarioDto>().ReverseMap();
        }
    }
}
