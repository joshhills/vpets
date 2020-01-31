using AutoMapper;
using VPets.Domain.Models;
using VPets.Domain.Models.Pets;
using VPets.Resources;

namespace VPets.Mapping
{
    /// <summary>
    /// Define how DTOs from the client can be mapped to internal models
    /// private to the application context.
    /// </summary>
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateUserResource, User>();
            CreateMap<CreatePetResource, Cat>();
            CreateMap<CreatePetResource, Dog>();
        }
    }
}
