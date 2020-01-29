using System;
using System.Collections.Generic;
using VPets.Models;
using static VPets.Domain.Models.Metric;

namespace VPets.Domain.Models
{
    public abstract class Pet : Entity
    {
        public string Name { get; set; }
        public PetType Type { get; set; }
        public Dictionary<MetricType, Metric> Metrics { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        protected Pet(PetType type, Dictionary<MetricType, Metric> metrics)
        {
            Type = type;
            Metrics = metrics;
        }
    }

    public enum PetType
    {
        CAT,
        DOG
    }
}
