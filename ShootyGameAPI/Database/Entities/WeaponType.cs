using ShootyGameAPI.Database.Entities.Interfaces;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Database.Entities
{
    public class WeaponType : ISoftDelete
    {
        // Properties
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
        public EquipmentSlot EquipmentSlot { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public List<Weapon> Weapons { get; set; }
    }
}
