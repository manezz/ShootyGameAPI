using ShootyGameAPI.Database.Entities.Interfaces;

namespace ShootyGameAPI.Database.Entities
{
    public class WeaponType : ISoftDelete
    {
        // Properties
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
        public int EquipmentSlot { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public List<Weapon> Weapons { get; set; }
    }
}
