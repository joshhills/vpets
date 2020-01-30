using System;
using System.ComponentModel;

namespace VPets.Domain.Models
{
    public abstract class Metric
    {
        public bool Ascending { get; set; }
        public float DeltaPerSecond { get; set; }
        public float Max { get; set; }
        public float Min { get; set; }
        public float Value { get; set; }
        public DateTime DateUpdated { get; set; }

        // Values could come from config...
        protected Metric(bool ascending = true, float deltaPerSecond = 0.05F, float max = 1, float min = 0, float value = 0.5F)
        {
            Ascending = ascending;
            DeltaPerSecond = deltaPerSecond;
            Max = max;
            Min = min;
            Value = value;

            DateUpdated = DateTime.Now;
        }

        public enum MetricType
        {
            [Description("hunger")]
            HUNGER = 1,

            [Description("happiness")]
            HAPPINESS = 2
        }

        protected virtual void Degrade()
        {
            DateTime now = DateTime.Now;

            float elapsed = now.Millisecond - DateUpdated.Millisecond / 1000;

            float deltaValue = elapsed * DeltaPerSecond;

            if (Ascending)
            {
                Value += deltaValue;
                Value = Value > Max ? Max : Value;
            } else
            {
                Value -= deltaValue;
                Value = Value < Min ? Min : Value;
            }

            DateUpdated = now;
        }
    }
}
