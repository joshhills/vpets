using System.Collections.Generic;
using System.ComponentModel;
using static VPets.Domain.Models.Metric;

namespace VPets.Domain.Models
{
    /// <summary>
    /// Base type for Pets.
    /// </summary>
    /// <remarks>
    /// Abstract so Pets must have a species to be instantiated. 
    /// </remarks>
    public abstract class Pet : Entity
    {
        /// <summary>
        /// The Pet's display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A descriptor for a Pet's derived type.
        /// </summary>
        /// <remarks>
        /// Used to distinguish client requests.
        /// </remarks>
        public PetType Type { get; set; }

        /// <summary>
        /// All metrics being tracked for this Pet.
        /// </summary>
        public Dictionary<MetricType, Metric> Metrics { get; set; }

        /// <summary>
        /// The Id of the User this Pet belongs to - required by EFC.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The User this Pet belongs to - required by EFC.
        /// </summary>
        public User User { get; set; }

        protected Pet(PetType type, Dictionary<MetricType, Metric> metrics)
        {
            Type = type;
            Metrics = metrics;
        }
    }

    /// <summary>
    /// Common pattern to constrain sub-types.
    /// </summary>
    public enum PetType
    {
        [Description("Cat")]
        CAT = 1,

        [Description("Dog")]
        DOG = 2
    }
}
