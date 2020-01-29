using System;
namespace VPets.Domain.Models.Metrics
{
    public class HappinessMetric : Metric
    {
        public HappinessMetric(float deltaPerSecond = 0.05F) : base(false, deltaPerSecond) { }
    }
}
