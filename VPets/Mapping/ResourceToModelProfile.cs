using System;
using AutoMapper;
using VPets.Models;
using VPets.Resources;

namespace VPets.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<UserResource, User>();
        }
    }
}
