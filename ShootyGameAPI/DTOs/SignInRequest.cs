using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class SignInRequest
    {
        [Required]
        [StringLength(64, ErrorMessage = "Cannot be longer than 64 chars.")]
        public string Email { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Cannot be longer than 64 chars.")]
        public string Password { get; set; }
    }
}
