using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPI.Services
{
    public interface IFriendReqService
    {
        Task<List<FriendReqResponse>> FindFriendReqByRequesterIdAsync(int requesterId);
        Task<List<FriendReqResponse>> FindFriendReqByReceiverIdAsync(int receiverId);
        Task<FriendReqResponse?> FindFriendReqByIdAsync(int friendRequestId);
        Task<FriendReqResponse?> CreateFriendReqAsync(FriendReqRequest newFriendRequest);
        Task<FriendReqResponse?> UpdateFriendReqByIdAsync(int friendRequestId, FriendReqUpdateRequest friendRequestUpdateRequest);
        Task<FriendReqResponse?> DeleteFriendReqByIdAsync(int friendRequestId);
    }

    public class FriendReqService : IFriendReqService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserService _userService;

        public FriendReqService(IFriendRequestRepository friendRequestRepository, IUserService userService)
        {
            _friendRequestRepository = friendRequestRepository;
            _userService = userService;
        }

        private static FriendReqResponse MapFriendReqToFriendRequestResponse(FriendReq friendRequest)
        {
            return new FriendReqResponse
            {
                FriendRequestId = friendRequest.FriendReqId,
                RequesterId = friendRequest.RequesterId,
                ReceiverId = friendRequest.ReceiverId,
                Status = friendRequest.Status,
                Requester = new FriendReq_UserResponse
                {
                    UserId = friendRequest.Requester.UserId,
                    UserName = friendRequest.Requester.UserName,
                    PlayerTag = friendRequest.Requester.PlayerTag
                },
                Receiver = new FriendReq_UserResponse
                {
                    UserId = friendRequest.Receiver.UserId,
                    UserName = friendRequest.Receiver.UserName,
                    PlayerTag = friendRequest.Receiver.PlayerTag
                }
            };
        }

        private static FriendReq MapFriendReqRequestToFriendRequest(FriendReqRequest friendRequestRequest)
        {
            return new FriendReq
            {
                RequesterId = friendRequestRequest.RequesterId,
                ReceiverId = friendRequestRequest.ReceiverId,
                ResponseAt = DateTime.Now
            };
        }

        private static FriendReq MapFriendReqRequestToUpdateFriendRequest(FriendReqUpdateRequest friendRequestUpdateRequest)
        {
            return new FriendReq
            {
                Status = friendRequestUpdateRequest.Status,
                ResponseAt = DateTime.Now
            };
        }

        public async Task<List<FriendReqResponse>> FindFriendReqByRequesterIdAsync(int requesterId)
        {
            var friendRequests = await _friendRequestRepository.FindAllFriendReqByRequesterIdAsync(requesterId);
            return friendRequests.Select(MapFriendReqToFriendRequestResponse).ToList();
        }

        public async Task<List<FriendReqResponse>> FindFriendReqByReceiverIdAsync(int receiverId)
        {
            var friendRequests = await _friendRequestRepository.FindAllFriendReqByReceiverIdAsync(receiverId);
            return friendRequests.Select(MapFriendReqToFriendRequestResponse).ToList();
        }

        public async Task<FriendReqResponse?> FindFriendReqByIdAsync(int friendRequestId)
        {
            var friendRequest = await _friendRequestRepository.FindFriendReqByIdAsync(friendRequestId);

            if (friendRequest == null)
            {
                return null;
            }

            return MapFriendReqToFriendRequestResponse(friendRequest);
        }

        public async Task<FriendReqResponse?> CreateFriendReqAsync(FriendReqRequest newFriendRequest)
        {
            if (newFriendRequest.RequesterId == newFriendRequest.ReceiverId)
            {
                throw new InvalidOperationException("You cannot send a friend request to yourself.");
            }

            var oldFriendReq = await _friendRequestRepository.FindFriendReqByRequesterIdAndReceiverIdAsync(newFriendRequest.RequesterId, newFriendRequest.ReceiverId);

            if (oldFriendReq != null)
            {
                throw new InvalidOperationException("There is already a pending friend request.");
            }

            var oldFriends = await _userService.FindFriendByIdAsync(newFriendRequest.RequesterId, newFriendRequest.ReceiverId);

            if (oldFriends != null)
            {
                throw new InvalidOperationException("You are already friends.");
            }

            var friendReq = MapFriendReqRequestToFriendRequest(newFriendRequest);
            var createdFriendRequest = await _friendRequestRepository.CreateFriendReqAsync(friendReq);

            if (createdFriendRequest == null)
            {
                return null;
            }

            return MapFriendReqToFriendRequestResponse(createdFriendRequest);
        }

        public async Task<FriendReqResponse?> UpdateFriendReqByIdAsync(int friendRequestId, FriendReqUpdateRequest friendReqUpdateRequest)
        {
            var existingFriendRequest = await _friendRequestRepository.FindFriendReqByIdAsync(friendRequestId);
            if (existingFriendRequest == null)
            {
                return null;
            }

            var existingStatus = existingFriendRequest.Status;

            var friendReq = MapFriendReqRequestToUpdateFriendRequest(friendReqUpdateRequest);
            var updatedFriendReq = await _friendRequestRepository.UpdateFriendReqByIdAsync(friendRequestId, friendReq);

            if (updatedFriendReq == null)
            {
                return null;
            }

            var friendRequest = new FriendRequest
            {
                RequesterId = updatedFriendReq.RequesterId,
                ReceiverId = updatedFriendReq.ReceiverId
            };

            // If status changed to Accepted, trigger AddFriendToUserAsync
            if (existingStatus != updatedFriendReq.Status &&
                updatedFriendReq.Status == FriendReqStatus.Accepted)
            {
                await _userService.AddFriendToUserAsync(updatedFriendReq.RequesterId, friendRequest);
            }

            return MapFriendReqToFriendRequestResponse(updatedFriendReq);
        }

        public async Task<FriendReqResponse?> DeleteFriendReqByIdAsync(int friendRequestId)
        {

            var deletedFriendRequest = await _friendRequestRepository.DeleteFriendReqByIdAsync(friendRequestId);

            if (deletedFriendRequest == null)
            {
                return null;
            }

            return MapFriendReqToFriendRequestResponse(deletedFriendRequest);
        }
    }
}
