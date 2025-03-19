using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Repositorys
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> FindUserByIdAsync(int userId);
        Task<User?> FindUserByEmailAsync(string email);
        Task<User?> CreateUserAsync(User newUser);
        Task<User?> UpdateUserByIdAsync(int userId, User updatedUser);
        Task<User?> DeleteUserByIdAsync(int userId);
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
            return await _context.Users
                .Include(x => x.UserWeapons)
                    .ThenInclude(x => x.Weapon)
                        .ThenInclude(x => x.WeaponType)
                .Include(x => x.SentFriendReqs.Where(f => f.Status == FriendReqStatus.Pending))
                .Include(x => x.ReceivedFriendReqs.Where(f => f.Status == FriendReqStatus.Pending))
                .Include(x => x.FriendsAsRequester)
                    .ThenInclude(x => x.Receiver)
                        .ThenInclude(x => x.Scores.OrderByDescending(x => x.ScoreValue))
                .Include(x => x.FriendsAsReceiver)
                    .ThenInclude(x => x.Requester)
                        .ThenInclude(x => x.Scores.OrderByDescending(x => x.ScoreValue))
                .Include(x => x.Scores.OrderByDescending(x => x.ScoreValue))
                .ToListAsync();
        }

        public async Task<User?> FindUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(x => x.UserWeapons)
                    .ThenInclude(x => x.Weapon)
                        .ThenInclude(x => x.WeaponType)
                .Include(x => x.SentFriendReqs.Where(f => f.Status == FriendReqStatus.Pending))
                .Include(x => x.ReceivedFriendReqs.Where(f => f.Status == FriendReqStatus.Pending))
                .Include(x => x.FriendsAsRequester)
                    .ThenInclude(x => x.Receiver)
                        .ThenInclude(x => x.Scores.OrderByDescending(x => x.ScoreValue))
                .Include(x => x.FriendsAsReceiver)
                    .ThenInclude(x => x.Requester)
                        .ThenInclude(x => x.Scores.OrderByDescending(x => x.ScoreValue))
                .Include(x => x.Scores.OrderByDescending(x => x.ScoreValue))
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> CreateUserAsync(User newUser)
        {
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return await FindUserByIdAsync(newUser.UserId);
        }

        public async Task<User?> UpdateUserByIdAsync(int userId, User updatedUser)
        {
            var user = await FindUserByIdAsync(userId);

            if (user != null)
            {
                user.Email = updatedUser.Email;
                user.UserName = updatedUser.UserName;
                user.PlayerTag = updatedUser.PlayerTag;
                user.PasswordHash = updatedUser.PasswordHash;

                await _context.SaveChangesAsync();
                return await FindUserByIdAsync(userId);
            }
            return user;
        }

        public async Task<User?> DeleteUserByIdAsync(int userId)
        {
            var user = await FindUserByIdAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
    }
}
