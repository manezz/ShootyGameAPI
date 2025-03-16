using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IFriendRepository
    {
        Task<List<Friend>> FindAllFriendsByUserIdAsync(int userId);
        Task<Friend?> FindFriendByIdAsync(int RequesterId, int ReceiverId);
        Task<Friend> CreateFriendAsync(Friend newFriend);
        Task<Friend?> DeleteFriendByIdAsync(int RequesterId, int ReceiverId);
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
            return await _context.Friends.Where(x => x.RequesterId == userId || x.ReceiverId == userId).ToListAsync();
        }

        // cheacks both ways
        public async Task<Friend?> FindFriendByIdAsync(int RequesterId, int ReceiverId)
        {
            return await _context.Friends
                .FirstOrDefaultAsync(x => (x.RequesterId == RequesterId && x.ReceiverId == ReceiverId)
                || (x.RequesterId == ReceiverId && x.ReceiverId == RequesterId));
        }

        public async Task<Friend> CreateFriendAsync(Friend newFriend)
        {
            _context.Friends.Add(newFriend);
            await _context.SaveChangesAsync();
            return newFriend;
        }

        public async Task<Friend?> DeleteFriendByIdAsync(int RequesterId, int ReceiverId)
        {
            var friend = await FindFriendByIdAsync(RequesterId, ReceiverId);
            if (friend != null)
            {
                _context.Friends.Remove(friend);
                await _context.SaveChangesAsync();
            }
            return friend;
        }
    }
}
