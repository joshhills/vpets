using System;

namespace VPets.Domain.Models
{
    /// <summary>
    /// Superclass to every entity in data-store.
    /// </summary>
    public class Entity
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
