using Moq;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ServiceTests
{
    public class FriendReqServiceTests
    {
        private readonly FriendReqService _friendReqService;
        private readonly Mock<IFriendRequestRepository> _friendRequestRepositoryMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();

        public FriendReqServiceTests()
        {
            _friendReqService = new FriendReqService(
                _friendRequestRepositoryMock.Object,
                _userServiceMock.Object);
        }

        [Fact]
        public async Task FindFriendReqByRequesterIdAsync_ShouldReturnList_WhenRequestsExist()
        {
            // Arrange
            int requesterId = 1;

            var friendReqs = new List<FriendReq>
            {
                new()
                {
                    FriendReqId = 1,
                    RequesterId = requesterId,
                    ReceiverId = 2,
                    Status = FriendReqStatus.Pending,
                    Requester = new User(),
                    Receiver = new User()
                },
                new()
                {
                    FriendReqId = 2,
                    RequesterId = requesterId,
                    ReceiverId = 3,
                    Status = FriendReqStatus.Pending,
                    Requester = new User(),
                    Receiver = new User()
                }
            };

            _friendRequestRepositoryMock
                .Setup(x => x.FindAllFriendReqByRequesterIdAsync(requesterId))
                .ReturnsAsync(friendReqs);

            // Act
            var result = await _friendReqService.FindFriendReqByRequesterIdAsync(requesterId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendReqResponse>>(result);
            Assert.Equal(2, result?.Count);
            Assert.All(result!, r => Assert.Equal(requesterId, r.RequesterId));
        }

        [Fact]
        public async Task FindFriendReqByReceiverIdAsync_ShouldReturnList_WhenRequestsExist()
        {
            // Arrange
            int receiverId = 1;

            var friendReqs = new List<FriendReq>
            {
                new()
                {
                    FriendReqId = 1,
                    RequesterId = 2,
                    ReceiverId = receiverId,
                    Status = FriendReqStatus.Pending,
                    Requester = new User(),
                    Receiver = new User()
                }
            };

            _friendRequestRepositoryMock
                .Setup(x => x.FindAllFriendReqByReceiverIdAsync(receiverId))
                .ReturnsAsync(friendReqs);

            // Act
            var result = await _friendReqService.FindFriendReqByReceiverIdAsync(receiverId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendReqResponse>>(result);
            Assert.Single(result);
            Assert.Equal(receiverId, result?.First().ReceiverId);
        }

        [Fact]
        public async Task FindFriendReqByIdAsync_ShouldReturnFriendReqResponse_WhenRequestExists()
        {
            // Arrange
            int reqId = 1;

            var friendReq = new FriendReq
            {
                FriendReqId = reqId,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Pending,
                Requester = new User(),
                Receiver = new User()
            };

            _friendRequestRepositoryMock
                .Setup(x => x.FindFriendReqByIdAsync(reqId))
                .ReturnsAsync(friendReq);

            // Act
            var result = await _friendReqService.FindFriendReqByIdAsync(reqId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FriendReqResponse>(result);
            Assert.Equal(reqId, result?.FriendRequestId);
        }

        [Fact]
        public async Task FindFriendReqByIdAsync_ShouldReturnNull_WhenRequestDoesNotExist()
        {
            // Arrange
            int reqId = 1;

            _friendRequestRepositoryMock
                .Setup(x => x.FindFriendReqByIdAsync(reqId))
                .ReturnsAsync(() => null!);

            // Act
            var result = await _friendReqService.FindFriendReqByIdAsync(reqId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateFriendRequestAsync_ShouldReturnFriendReqResponse_WhenCreationSucceeds()
        {
            // Arrange
            var request = new FriendReqRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            var createdFriendReq = new FriendReq
            {
                FriendReqId = 1,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Pending,
                Requester = new User(),
                Receiver = new User()
            };

            _friendRequestRepositoryMock
                .Setup(x => x.CreateFriendReqAsync(It.IsAny<FriendReq>()))
                .ReturnsAsync(createdFriendReq);

            // Act
            var result = await _friendReqService.CreateFriendReqAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FriendReqResponse>(result);
            Assert.Equal(request.RequesterId, result?.RequesterId);
            Assert.Equal(request.ReceiverId, result?.ReceiverId);
        }

        [Fact]
        public async Task CreateFriendRequestAsync_ShouldReturnNull_WhenCreationFails()
        {
            // Arrange
            var request = new FriendReqRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            _friendRequestRepositoryMock
                .Setup(x => x.CreateFriendReqAsync(It.IsAny<FriendReq>()))
                .ReturnsAsync(() => null!);

            // Act
            var result = await _friendReqService.CreateFriendReqAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateFriendRequestByIdAsync_ShouldReturnFriendReqResponse_WhenUpdateSucceeds()
        {
            // Arrange
            int reqId = 1;

            var updateRequest = new FriendReqUpdateRequest
            {
                Status = FriendReqStatus.Accepted
            };

            var existingFriendReq = new FriendReq
            {
                FriendReqId = reqId,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Pending, // previous status
                Requester = new User(),
                Receiver = new User()
            };

            var updatedFriendReq = new FriendReq
            {
                FriendReqId = reqId,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Accepted,
                Requester = new User(),
                Receiver = new User()
            };

            _friendRequestRepositoryMock
                .Setup(x => x.FindFriendReqByIdAsync(reqId))
                .ReturnsAsync(existingFriendReq);

            _friendRequestRepositoryMock
                .Setup(x => x.UpdateFriendReqByIdAsync(It.IsAny<int>(), It.IsAny<FriendReq>()))
                .ReturnsAsync(updatedFriendReq);

            // Act
            var result = await _friendReqService.UpdateFriendReqByIdAsync(reqId, updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FriendReqResponse>(result);
            Assert.Equal(updateRequest.Status, result?.Status);
        }

        [Fact]
        public async Task UpdateFriendRequestByIdAsync_ShouldReturnNull_WhenUpdateFails()
        {
            // Arrange
            int reqId = 1;

            var updateRequest = new FriendReqUpdateRequest
            {
                Status = FriendReqStatus.Declined
            };

            _friendRequestRepositoryMock
                .Setup(x => x.UpdateFriendReqByIdAsync(It.IsAny<int>(), It.IsAny<FriendReq>()))
                .ReturnsAsync(() => null!);

            // Act
            var result = await _friendReqService.UpdateFriendReqByIdAsync(reqId, updateRequest);

            // Assert
            Assert.Null(result);
        }
    }
}
