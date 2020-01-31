using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VPets.Resources
{
    /// <summary>
    /// Display a Pet that has just been created to the client.
    /// </summary>
    /// <remarks>
    /// The User has been omitted as it is irrelevant to the client.
    /// </remarks>
    public class CreatedPetResource
    {
        /// <summary>
        /// Unique Id as auto-incrementing integer.
        /// </summary>
        /// <example>1</example>
        [Required]
        public int Id { get; set; }

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
        /// Embed the user to allow for navigation.
        /// </summary>
        /// <example>
        /// { "id": 1, "name": "Josh" }
        /// </example>
        [Required]
        public UserResource User { get; set; }

        /// <summary>
        /// The type of Pet.
        /// </summary>
        /// <example>Dog</example>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Metric names mapped to their values representing the state of the Pet.
        /// </summary>
        /// <example>
        /// { "hunger": 1.0 }
        /// </example>
        /// <remarks>
        /// These are converted from a more complex internal state
        /// (to prevent cheating!).
        ///
        /// Their could be different metrics per-animal.
        /// </remarks>
        [Required]
        public Dictionary<string, float> Metrics { get; set; }
    }
}
