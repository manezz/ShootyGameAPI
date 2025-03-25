using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IWeaponTypeRepository
    {
        Task<List<WeaponType>> GetAllWeaponTypesAsync();
        Task<WeaponType?> FindWeaponTypeByIdAsync(int weaponTypeId);
        Task<WeaponType> CreateWeaponTypeAsync(WeaponType newWeaponType);
        Task<WeaponType?> UpdateWeaponTypeByIdAsync(int weaponTypeId, WeaponType updatedWeaponType);
        Task<WeaponType?> DeleteWeaponTypeByIdAsync(int weaponTypeId);
    }

    public class WeaponTypeRepository : IWeaponTypeRepository
    {
        private readonly DatabaseContext _context;

        public WeaponTypeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<WeaponType>> GetAllWeaponTypesAsync()
        {
            return await _context.WeaponTypes.ToListAsync();
        }

        public async Task<WeaponType?> FindWeaponTypeByIdAsync(int weaponTypeId)
        {
            return await _context.WeaponTypes.FindAsync(weaponTypeId);
        }

        public async Task<WeaponType> CreateWeaponTypeAsync(WeaponType newWeaponType)
        {
            _context.WeaponTypes.Add(newWeaponType);
            await _context.SaveChangesAsync();
            return newWeaponType;
        }

        public async Task<WeaponType?> UpdateWeaponTypeByIdAsync(int weaponTypeId, WeaponType updatedWeaponType)
        {
            var weaponType = await FindWeaponTypeByIdAsync(weaponTypeId);

            if (weaponType != null)
            {
                weaponType.Name = updatedWeaponType.Name;
                weaponType.EquipmentSlot = updatedWeaponType.EquipmentSlot;

                await _context.SaveChangesAsync();
                return await FindWeaponTypeByIdAsync(weaponTypeId);
            }
            return weaponType;
        }

        public async Task<WeaponType?> DeleteWeaponTypeByIdAsync(int weaponTypeId)
        {
            var weaponType = await FindWeaponTypeByIdAsync(weaponTypeId);
            if (weaponType != null)
            {
                _context.WeaponTypes.Remove(weaponType);
                await _context.SaveChangesAsync();
            }
            return weaponType;
        }
    }
}
