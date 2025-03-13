using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IFriendRepository
    {
        public Task<List<Friend>> FindAllFriendsByUserIdAsync(int userId);
        public Task<Friend?> FindFriendByIdAsync(int user1Id, int user2Id);
        public Task<Friend> CreateFriendAsync(Friend newFriend);
        public Task<Friend?> DeleteFriendByIdAsync(int user1Id, int user2Id);
    }

    public class FriendRepository : IFriendRepository
    {
        private readonly DatabaseContext _context;
        public FriendRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Friend>> FindAllFriendsByUserIdAsync(int userId)
        {
            return await _context.Friends.Where(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
        }

        public async Task<Friend?> FindFriendByIdAsync(int user1Id, int user2Id)
        {
            return await _context.Friends.FindAsync(user1Id, user2Id);
        }

        public async Task<Friend> CreateFriendAsync(Friend newFriend)
        {
            _context.Friends.Add(newFriend);
            await _context.SaveChangesAsync();
            return newFriend;
        }

        public async Task<Friend?> DeleteFriendByIdAsync(int user1Id, int user2Id)
        {
            var friend = await FindFriendByIdAsync(user1Id, user2Id);
            if (friend != null)
            {
                _context.Friends.Remove(friend);
                await _context.SaveChangesAsync();
            }
            return friend;
        }
    }
}
