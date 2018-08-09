using AutoMapper;
using ClientModels;
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
            CreateMap<UpdateLocationMessage, Location>()
                .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.LocationX))
                .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.LocationY))
                .ForMember(dest => dest.Z, opt => opt.MapFrom(src => src.LocationZ));
        }
    }
}
