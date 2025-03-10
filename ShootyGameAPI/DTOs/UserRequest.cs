using ShootyGameAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class UserRequest
    {
        [Required]
        [StringLength(64, ErrorMessage = "Cannot be longer than 64 chars.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Cannot be longer than 64 chars.")]
        public string Email { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Cannot be longer than 64 chars.")]
        public string Password { get; set; }

        public Role Role { get; set; }
    }
}
