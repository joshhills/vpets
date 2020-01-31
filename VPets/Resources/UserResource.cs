using System.ComponentModel.DataAnnotations;

namespace VPets.Resources
{
    /// <summary>
    /// Display a User to the client.
    /// </summary>
    /// <remarks>
    /// Creation date has been omitted as it is irrelevant to the client.
    /// </remarks>
    public class UserResource
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
        /// <example>Josh</example>
        [Required]
        [MinLength(3)]
        [MaxLength(32)]
        [RegularExpression(@"^[a-zA-Z''-'\s]*$", ErrorMessage = "Must be simple characters and no extra spacing")]
        public string Name { get; set; }
    }
}
