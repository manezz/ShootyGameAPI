﻿using ShootyGameAPI.Database.Entities.Interfaces;

namespace ShootyGameAPI.Database.Entities
{
    public class UserWeapon : ISoftDelete
    {
        // Properties
        public int UserId { get; set; }
        public int WeaponId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Weapon Weapon { get; set; }
    }
}
