using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IFriendRequestRepository
    {
        public Task<List<FriendRequest>> FindAllFriendRequestsByRequesterIdAsync(int requesterId);
        public Task<List<FriendRequest>> FindAllFriendRequestsByReceiverIdAsync(int receiverId);
        public Task<FriendRequest?> FindFriendRequestByIdAsync(int friendRequestId);
        public Task<FriendRequest> CreateFriendRequestAsync(FriendRequest newFriendRequest);
        public Task<FriendRequest?> DeleteFriendRequestByIdAsync(int friendRequestId);
    }

    public class FriendRequestRepository : IFriendRequestRepository
    {
        public readonly DatabaseContext _context;

        public FriendRequestRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<FriendRequest>> FindAllFriendRequestsByRequesterIdAsync(int requesterId)
        {
            return await _context.FriendRequests.Where(f => f.RequesterId == requesterId).ToListAsync();
        }

        public async Task<List<FriendRequest>> FindAllFriendRequestsByReceiverIdAsync(int receiverId)
        {
            return await _context.FriendRequests.Where(f => f.ReceiverId == receiverId).ToListAsync();
        }

        public async Task<FriendRequest?> FindFriendRequestByIdAsync(int friendRequestId)
        {
            return await _context.FriendRequests.FindAsync(friendRequestId);
        }

        public async Task<FriendRequest> CreateFriendRequestAsync(FriendRequest newFriendRequest)
        {
            _context.FriendRequests.Add(newFriendRequest);
            await _context.SaveChangesAsync();
            return newFriendRequest;
        }

        public async Task<FriendRequest?> DeleteFriendRequestByIdAsync(int friendRequestId)
        {
            var friendRequest = await FindFriendRequestByIdAsync(friendRequestId);
            if (friendRequest != null)
            {
                _context.FriendRequests.Remove(friendRequest);
                await _context.SaveChangesAsync();
            }
            return friendRequest;
        }
    }
}
