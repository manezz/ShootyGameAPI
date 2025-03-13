using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.DTOs
{
    public class WeaponTypeResponse
    {
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
        public EquipmentSlot EquipmentSlot { get; set; }
    }
}
