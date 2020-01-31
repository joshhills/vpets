using System.ComponentModel.DataAnnotations;

namespace VPets.Resources
{
    /// <summary>
    /// Send a request to create a User to the server.
    /// </summary>
    public class CreateUserResource
    {
        /// <summary>
        /// Human-readable name, non-unique i.e. 'display-name'.
        /// </summary>
        /// <example>Josh</example>
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "Must be simple characters and no extra spacing")]
        public string Name { get; set; }
    }
}
