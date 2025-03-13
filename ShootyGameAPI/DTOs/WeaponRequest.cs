using ShootyGameAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class WeaponRequest
    {
        [Required]
        [StringLength(64, ErrorMessage = "Name cannot be longer than 64 characters.")]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public int Price { get; set; }

        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Reload Speed must be at minimum 0.1 seconds.")]
        public float ReloadSpeed { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Magazine Size must be at minimum 1.")]
        public int MagSize { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Fire Rate must be at 1 rounds per minute.")]
        public int FireRate { get; set; }

        [Required]
        public FireMode FireMode { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "WeaponTypeID must be a positive value.")]
        public int WeaponTypeId { get; set; }
    }
}
