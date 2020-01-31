namespace VPets.Domain.Models.Metrics
{
    public class HungerMetric : Metric
    {
        public HungerMetric(float deltaPerSecond = 0.05F) : base(true, deltaPerSecond) { }
    }
}
