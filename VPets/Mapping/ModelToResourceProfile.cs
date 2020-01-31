using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VPets.Domain.Models;
using VPets.Extensions;
using VPets.Resources;

namespace VPets.Mapping
{
    /// <summary>
    /// Define how internal models private to the application context
    /// can be mapped to DTOs for the client.
    /// </summary>
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<User, UserResource>();

            // Some duplicate code here could be refactored with a ResolutionContext
            CreateMap<Pet, PetResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()))
                .ForMember(src => src.Metrics,
                           SimplifyMetrics<PetResource>());

            CreateMap<Pet, UserPetResource>()
                .ForMember(src => src.Type,
                           opt => opt.MapFrom(src => src.Type.ToDescriptionString()))
                .ForMember(src => src.Metrics,
                           SimplifyMetrics<UserPetResource>());
        }

        /// <summary>
        /// Separate the functionality for flattening metrics dictionaries into
        /// string-float mappings - this also accomplishes computing the most
        /// up-to-date values for each metric for the sake of correctness
        /// *without*  database interaction. Given more information, the client
        /// could perform this itself.
        /// </summary>
        /// <returns>An Automapper configuration object</returns>
        private static Action<IMemberConfigurationExpression<Pet, T, Dictionary<string, float>>> SimplifyMetrics<T>()
        {
            return opt => opt.MapFrom(src => src.Metrics.ToDictionary(
                x => x.Key.ToDescriptionString(),
                // Pre-compute the correct metric value at the time of invocation
                // and round it to two decimal places for clarity.
                x => Math.Round(x.Value.Degrade().Value, 2)));
        }
    }
}
