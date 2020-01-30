using System;
using System.ComponentModel.DataAnnotations;

namespace VPets.Resources
{
    public class CreateUserResource
    {
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
