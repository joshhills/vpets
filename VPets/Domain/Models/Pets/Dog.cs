using System.Collections.Generic;
using VPets.Domain.Models.Metrics;
using static VPets.Domain.Models.Metric;

namespace VPets.Domain.Models.Pets
{
    public class Dog : Pet
    {
        public Dog() : base(
            PetType.DOG,
            new Dictionary<MetricType, Metric>() {
                { MetricType.HUNGER, new HungerMetric(0.04F) },
                { MetricType.HAPPINESS, new HappinessMetric(0.075F) }
            }
        )
        { }
    }
}
