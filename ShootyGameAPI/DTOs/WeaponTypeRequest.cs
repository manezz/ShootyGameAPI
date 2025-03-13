using ShootyGameAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class WeaponTypeRequest
    {
        [Required]
        [StringLength(64, ErrorMessage = "Cannot be longer than 64 chars.")]
        public string Name { get; set; }

        [Required]
        public EquipmentSlot EquipmentSlot { get; set; }
    }
}
