using AutoMapper;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.DistressSignalModel, Entities.Models.DistressSignal>();
            CreateMap<ClientModels.Location, Location>()
                .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.StarPosX))
                .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.StarPosY))
                .ForMember(dest => dest.Z, opt => opt.MapFrom(src => src.StarPosZ))
                .ForMember(dest => dest.SystemName, opt => opt.MapFrom(src => src.StarSystem));
        }
    }
}
