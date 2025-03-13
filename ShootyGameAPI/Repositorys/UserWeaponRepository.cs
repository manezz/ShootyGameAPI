using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IUserWeaponRepository
    {
        public Task<List<UserWeapon>> FindAllUserWeaponsByUserIdAsync(int userId);
        public Task<UserWeapon?> FindUserWeaponByIdAsync(int userId, int weaponId);
        public Task<UserWeapon> CreateUserWeaponAsync(UserWeapon newUserWeapon);
        public Task<UserWeapon?> DeleteUserWeaponByIdAsync(int userId, int weaponId);
    }
    public class UserWeaponRepository : IUserWeaponRepository
    {
        private readonly DatabaseContext _context;

        public UserWeaponRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<UserWeapon>> FindAllUserWeaponsByUserIdAsync(int userId)
        {
            return await _context.UserWeapons.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<UserWeapon?> FindUserWeaponByIdAsync(int userId, int weaponId)
        {
            return await _context.UserWeapons.FindAsync(userId, weaponId);
        }

        public async Task<UserWeapon> CreateUserWeaponAsync(UserWeapon newUserWeapon)
        {
            _context.UserWeapons.Add(newUserWeapon);
            await _context.SaveChangesAsync();
            return newUserWeapon;
        }

        public async Task<UserWeapon?> DeleteUserWeaponByIdAsync(int userId, int weaponId)
        {
            var userWeapon = await FindUserWeaponByIdAsync(userId, weaponId);
            if (userWeapon != null)
            {
                _context.UserWeapons.Remove(userWeapon);
                await _context.SaveChangesAsync();
            }
            return userWeapon;
        }
    }
}
