using System;
using VPets.Domain.Models;

namespace VPets.Resources
{
    public class CreatePetResource
    {
        public string Name { get; set; }
        public PetType Type { get; set; }
        public int UserId { get; set; }
    }
}
