using System.Collections.Generic;

namespace VPets.Resources
{
    public class CreatedPetResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserResource User { get; set; }
        public string Type { get; set; }
        public Dictionary<string, float> Metrics { get; set; }
    }
}
