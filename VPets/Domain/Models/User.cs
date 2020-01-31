using System;
using System.Collections.Generic;

namespace VPets.Domain.Models
{
    /// <summary>
    /// A User of the system.
    /// </summary>
    public class User : Entity
    {
        /// <summary>
        /// The User's display name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Pets the user owns - required by EFC.
        /// </summary>
        public IList<Pet> Pets { get; set; } = new List<Pet>();

        public void ToList()
        {
            throw new NotImplementedException();
        }
    }
}
