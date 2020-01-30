using System;
using System.Linq;
using AutoMapper;
using VPets.Domain.Models;
using VPets.Domain.Models.Pets;
using VPets.Extensions;
using VPets.Models;
using VPets.Resources;

namespace VPets.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateUserResource, User>();
            CreateMap<CreatePetResource, Cat>();
            CreateMap<CreatePetResource, Dog>();

            CreateMap<Pet, CreatedPetResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDesriptionString()))
                .ForMember(src => src.Metrics,
                           opt => opt.MapFrom(
                               src => src.Metrics.ToDictionary(
                                   x => x.Key.ToDesriptionString(),
                                   x => Math.Round(x.Value.Value, 2))));

            CreateMap<User, UserResource>();
            CreateMap<Pet, PetResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDesriptionString()))
                .ForMember(src => src.Metrics,
                           opt => opt.MapFrom(
                               src => src.Metrics.ToDictionary(
                                   x => x.Key.ToDesriptionString(),
                                   x => Math.Round(x.Value.Degrade().Value, 2))));

            CreateMap<Pet, UserPetResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDesriptionString()))
                .ForMember(src => src.Metrics,
                           opt => opt.MapFrom(
                               src => src.Metrics.ToDictionary(
                                   x => x.Key.ToDesriptionString(),
                                   x => Math.Round(x.Value.Degrade().Value, 2))));
        }
    }
}
