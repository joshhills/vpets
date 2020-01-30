using System;
using System.Linq;
using AutoMapper;
using VPets.Domain.Models;
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
            CreateMap<User, UserResource>();
            CreateMap<Pet, PetResource>().ForMember(src => src.Type,
                opt => opt.MapFrom(src => src.Type.ToDesriptionString()))
                .ForMember(src => src.Metrics, opt => opt.MapFrom(src =>
                    src.Metrics.ToDictionary(x => x.Key.ToDesriptionString(), x => x.Value.Value)));
        }
    }
}
