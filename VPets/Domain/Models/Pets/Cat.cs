using System.Collections.Generic;
using VPets.Domain.Models.Metrics;
using static VPets.Domain.Models.Metric;

namespace VPets.Domain.Models.Pets
{
    /// <summary>
    /// Cats become hungry faster than they become unhappy...
    /// </summary>
    /// <remarks>
    /// Since this is a POCO it can be extended with extra metrics,
    /// fields, methods etc.
    /// </remarks>
    public class Cat : Pet
    {
        public Cat() : base(
            PetType.CAT,
            new Dictionary<MetricType, Metric>() {
                { MetricType.HUNGER, new HungerMetric(0.1F) },
                { MetricType.HAPPINESS, new HappinessMetric(0.03F) }
            }
        ) { }
    }
}
