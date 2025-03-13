using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.DTOs
{
    public class WeaponResponse
    {
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public float ReloadSpeed { get; set; }
        public int MagSize { get; set; }
        public int FireRate { get; set; }
        public FireMode FireMode { get; set; }
        public Weapon_WeaponTypeResponse WeaponType { get; set; }
    }

    public class Weapon_WeaponTypeResponse
    {
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
        public EquipmentSlot EquipmentSlot { get; set; }
    }
}
