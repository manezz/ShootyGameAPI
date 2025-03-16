using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Repositorys
{
    public interface IFriendRequestRepository
    {
        Task<List<FriendReq>> FindAllFriendReqByRequesterIdAsync(int requesterId);
        Task<List<FriendReq>> FindAllFriendReqByReceiverIdAsync(int receiverId);
        Task<FriendReq?> FindFriendReqByIdAsync(int friendReqId);
        Task<FriendReq?> FindFriendReqByRequesterIdAndReceiverIdAsync(int requesterId, int receiverId);
        Task<FriendReq?> CreateFriendReqAsync(FriendReq newFriendReq);
        Task<FriendReq?> UpdateFriendReqByIdAsync(int friendReqId, FriendReq updatedFriendReq);
        Task<FriendReq?> DeleteFriendReqByIdAsync(int friendReqId);
    }

    public class FriendRequestRepository : IFriendRequestRepository
    {
        public readonly DatabaseContext _context;

        public FriendRequestRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<FriendReq>> FindAllFriendReqByRequesterIdAsync(int requesterId)
        {
            return await _context.FriendReqs.Where(f => f.RequesterId == requesterId)
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => f.Status == FriendReqStatus.Pending)
                .ToListAsync();
        }

        public async Task<List<FriendReq>> FindAllFriendReqByReceiverIdAsync(int receiverId)
        {
            return await _context.FriendReqs.Where(f => f.ReceiverId == receiverId)
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => f.Status == FriendReqStatus.Pending)
                .ToListAsync();
        }

        public async Task<FriendReq?> FindFriendReqByIdAsync(int friendReqId)
        {
            return await _context.FriendReqs
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => f.Status == FriendReqStatus.Pending)
                .FirstOrDefaultAsync(f => f.FriendReqId == friendReqId);
        }

        // cheacks both ways
        public async Task<FriendReq?> FindFriendReqByRequesterIdAndReceiverIdAsync(int requesterId, int receiverId)
        {
            return await _context.FriendReqs
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => f.Status == FriendReqStatus.Pending)
                .FirstOrDefaultAsync(f => (f.RequesterId == requesterId && f.ReceiverId == receiverId)
                || (f.RequesterId == receiverId && f.ReceiverId == requesterId));
        }

        public async Task<FriendReq?> CreateFriendReqAsync(FriendReq newFriendReq)
        {
            _context.FriendReqs.Add(newFriendReq);
            await _context.SaveChangesAsync();
            return await FindFriendReqByIdAsync(newFriendReq.FriendReqId);
        }

        public async Task<FriendReq?> UpdateFriendReqByIdAsync(int friendReqId, FriendReq updatedFriendReq)
        {
            var friendReq = await FindFriendReqByIdAsync(friendReqId);

            if (friendReq != null)
            {
                friendReq.Status = updatedFriendReq.Status;

                await _context.SaveChangesAsync();
                return friendReq;
            }
            return friendReq;
        }

        public async Task<FriendReq?> DeleteFriendReqByIdAsync(int friendReqId)
        {
            var friendReq = await FindFriendReqByIdAsync(friendReqId);
            if (friendReq != null)
            {
                _context.FriendReqs.Remove(friendReq);
                await _context.SaveChangesAsync();
            }
            return friendReq;
        }
    }
}
