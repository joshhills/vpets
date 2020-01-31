using System;
using System.ComponentModel;

namespace VPets.Domain.Models
{
    /// <summary>
    /// A trackable metric that tends towards a value over time in steps -
    /// the final value without interaction is assumed to be the "worst"
    /// value.
    /// </summary>
    public abstract class Metric
    {
        /// <summary>
        /// Whether the value should increase or decrease.
        /// </summary>
        public bool Ascending { get; set; }

        /// <summary>
        /// How much the value should change per second.
        /// </summary>
        public float DeltaPerSecond { get; set; }

        /// <summary>
        /// The maximum value for Value.
        /// </summary>
        public float Max { get; set; }

        /// <summary>
        /// The minimum value for Value.
        /// </summary>
        public float Min { get; set; }

        /// <summary>
        /// The current value of this metric.
        /// </summary>
        /// <remarks>
        /// This has eventual consistency thanks to the DateUpdated field
        /// and Degrade method.
        /// </remarks>
        public float Value { get; set; }

        /// <summary>
        /// The time that Value was last updated, used to compute correct new
        /// value at any given point in time, and updated in tandem with value.
        /// </summary>
        public DateTime DateUpdated { get; set; }

        /// <summary>
        /// Sensible default are set for values - they could be sourced from
        /// external config.
        /// </summary>
        protected Metric(bool ascending = true, float deltaPerSecond = 0.05F, float max = 1, float min = 0, float value = 0.5F)
        {
            Ascending = ascending;
            DeltaPerSecond = deltaPerSecond;
            Max = max;
            Min = min;
            Value = value;

            DateUpdated = DateTime.Now;
        }

        /// <summary>
        /// Common pattern to constrain sub-types.
        /// </summary>
        public enum MetricType
        {
            [Description("hunger")]
            HUNGER = 1,

            [Description("happiness")]
            HAPPINESS = 2
        }

        /// <summary>
        /// Tend the value linearly towards it's "worst" state by the delta
        /// time since it was last updated.
        /// </summary>
        /// <remarks>
        /// Virtual in case derived Pet types require something
        /// more sophisticated.
        /// </remarks>
        /// <returns>This, for building</returns>
        public virtual Metric Degrade()
        {
            DateTime now = DateTime.Now;

            float elapsed = (float) (now - DateUpdated).TotalSeconds;

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

            return this;
        }

        /// <summary>
        /// Reset to "best" value based on direction.
        /// </summary>
        /// <remarks>
        /// Virtual in case derived Pet types require something
        /// more sophisticated.
        /// </remarks>
        /// <returns>This, for building</returns>
        public virtual Metric Improve()
        {
            Value = Ascending ? Min : Max;
            DateUpdated = DateTime.Now;

            return this;
        }
    }
}
