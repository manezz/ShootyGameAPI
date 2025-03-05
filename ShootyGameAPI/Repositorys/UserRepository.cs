using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> FindUserByIdAsync(int id);
        Task<User?> FindUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User newUser);
        Task<User?> UpdateUserByIdAsync(int userId, User updatedUser);
        Task<User?> DeleteUserByIdAsync(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> FindUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> CreateUserAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<User?> UpdateUserByIdAsync(int userId, User updatedUser)
        {
            var user = await FindUserByIdAsync(updatedUser.UserId);

            if (user != null)
            {
                user.Email = updatedUser.Email;
                user.UserName = updatedUser.UserName;
                user.PlayerTag = updatedUser.PlayerTag;
                user.PasswordHash = updatedUser.PasswordHash;

                await _context.SaveChangesAsync();
                return updatedUser;
            }
            return user;
        }

        public async Task<User?> DeleteUserByIdAsync(int id)
        {
            var user = await FindUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
    }
}
