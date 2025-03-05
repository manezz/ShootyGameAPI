using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IWeaponRepository
    {
        Task<List<Weapon>> GetAllWeaponsAsync();
        Task<Weapon?> FindWeaponByIdAsync(int id);
        Task<Weapon> CreateWeaponAsync(Weapon newWeapon);
        Task<Weapon?> UpdateWeaponByIdAsync(int weaponId, Weapon updatedWeapon);
        Task<Weapon?> DeleteWeaponByIdAsync(int id);
    }

    public class WeaponRepository : IWeaponRepository
    {
        private readonly DatabaseContext _context;

        public WeaponRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Weapon>> GetAllWeaponsAsync()
        {
            return await _context.Weapons.ToListAsync();
        }

        public async Task<Weapon?> FindWeaponByIdAsync(int id)
        {
            return await _context.Weapons.FindAsync(id);
        }

        public async Task<Weapon> CreateWeaponAsync(Weapon newWeapon)
        {
            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();
            return newWeapon;
        }

        public async Task<Weapon?> UpdateWeaponByIdAsync(int weaponId, Weapon updatedWeapon)
        {
            var weapon = await FindWeaponByIdAsync(updatedWeapon.WeaponId);

            if (weapon != null)
            {
                weapon.WeaponTypeId = updatedWeapon.WeaponTypeId;
                weapon.Name = updatedWeapon.Name;
                weapon.Price = updatedWeapon.Price;
                weapon.ReloadSpeed = updatedWeapon.ReloadSpeed;
                weapon.MagSize = updatedWeapon.MagSize;
                weapon.FireRate = updatedWeapon.FireRate;

                await _context.SaveChangesAsync();
                return updatedWeapon;
            }
            return weapon;
        }

        public async Task<Weapon?> DeleteWeaponByIdAsync(int id)
        {
            var weapon = await FindWeaponByIdAsync(id);
            if (weapon != null)
            {
                _context.Weapons.Remove(weapon);
                await _context.SaveChangesAsync();
            }
            return weapon;
        }
    }
}
