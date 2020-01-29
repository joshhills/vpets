using System;
using System.ComponentModel.DataAnnotations;

namespace VPets.Resources
{
    public class UserResource
    {
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
