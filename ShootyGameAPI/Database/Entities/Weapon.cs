using ShootyGameAPI.Database.Entities.Interfaces;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Database.Entities
{
    public class Weapon : ISoftDelete
    {
        // Properties
        public int WeaponId { get; set; }
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public float ReloadSpeed { get; set; }
        public int MagSize { get; set; }
        public int FireRate { get; set; }
        public FireMode FireMode { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public List<UserWeapon> UserWeapons { get; set; }
        public WeaponType WeaponType { get; set; }
    }
}
