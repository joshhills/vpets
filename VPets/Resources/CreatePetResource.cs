using System.ComponentModel.DataAnnotations;
using VPets.Domain.Models;

namespace VPets.Resources
{
    /// <summary>
    /// Send a request to create a Pet to the server.
    /// </summary>
    public class CreatePetResource
    {
        /// <summary>
        /// Human-readable name, non-unique i.e. 'display-name'.
        /// </summary>
        /// <example>Woofus</example>
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "Must be simple characters and no extra spacing")]
        public string Name { get; set; }

        /// <summary>
        /// The type of Pet.
        /// </summary>
        /// <example>Dog</example>
        [Required]
        public PetType Type { get; set; }

        /// <summary>
        /// The Id of the User who owns this Pet
        /// </summary>
        /// <example>1</example>
        /// <remarks>
        /// Must be a pre-existing User
        /// </remarks>
        [Required]
        public int UserId { get; set; }
    }
}
