using System.Collections.Generic;

namespace VPets.Domain.Models
{
    public class User : Entity
    {
        public string Name { get; set; }
        public IList<Pet> Pets { get; set; } = new List<Pet>();
    }
}
