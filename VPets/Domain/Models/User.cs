using System;
using System.Collections.Generic;
using VPets.Domain.Models;

namespace VPets.Models
{
    public class User : Entity
    {
        public string Name { get; set; }
        public IList<Pet> Pets { get; set; } = new List<Pet>();
    }
}
