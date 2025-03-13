using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class UserWeaponRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be at minimum 1.")]
        public int UserId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "WeaponId must be at minimum 1.")]
        public int WeaponId { get; set; }
    }
}
