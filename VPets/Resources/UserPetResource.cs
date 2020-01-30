using System.Collections.Generic;

namespace VPets.Resources
{
    public class UserPetResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Dictionary<string, float> Metrics { get; set; }
    }
}