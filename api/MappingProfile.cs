using AutoMapper;
using Entities.Interfaces;
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
            CreateMap<ClientModels.Location, Location>()
                .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.StarPosX))
                .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.StarPosY))
                .ForMember(dest => dest.Z, opt => opt.MapFrom(src => src.StarPosZ))
                .ForMember(dest => dest.SystemName, opt => opt.MapFrom(src => src.StarSystem));

            CreateMap<Location, ClientModels.Location>()
                .ForMember(dest => dest.StarPosX, opt => opt.MapFrom(src => src.X))
                .ForMember(dest => dest.StarPosY, opt => opt.MapFrom(src => src.Y))
                .ForMember(dest => dest.StarPosZ, opt => opt.MapFrom(src => src.Z))
                .ForMember(dest => dest.StarSystem, opt => opt.MapFrom(src => src.SystemName));

            CreateMap<ClientModels.PlayerInfoMessage, IPlayerInfo>()
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => src.ApiKey))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ShipType, opt => opt.MapFrom(src => src.ShipType));

            CreateMap<ClientModels.LogInMessage, IPlayerInfo>()
                .ForMember(dest => dest.ApiKey, opt => opt.MapFrom(src => src.ApiKey))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ShipType, opt => opt.MapFrom(src => src.ShipType));

        }
    }
}
