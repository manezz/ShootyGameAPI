using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using ShootyGameAPI.Controllers;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;
using System.Data;
using System.Net.Http;

namespace ShootyGameAPITests.ConrollerTests
{
    public class FriendReqControllerTests
    {
        private readonly FriendReqController _friendReqController;
        private readonly Mock<IFriendReqService> _friendReqServiceMock = new();
        private readonly HttpContext httpContext = new DefaultHttpContext();

        public FriendReqControllerTests()
        {
            _friendReqController = new FriendReqController(_friendReqServiceMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task FindFriendReqByRequesterIdAsync_ShouldReturnStatusCode200_WhenRequestsExist()
        {
            // Arrange
            int requesterId = 1;

            var userResponse = new UserResponse
            {
                UserId = requesterId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            var friendReqs = new List<FriendReqResponse>
            {
                new FriendReqResponse
                {
                    FriendRequestId = 1,
                    RequesterId = requesterId,
                    ReceiverId = 2,
                    Status = FriendReqStatus.Pending
                }
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByRequesterIdAsync(requesterId))
                .ReturnsAsync(friendReqs);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.FindFriendReqByRequesterIdAsync(requesterId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task FindFriendReqByRequesterIdAsync_ShouldReturnStatusCode204_WhenNoRequestsExist()
        {
            // Arrange
            int requesterId = 1;

            var userResponse = new UserResponse
            {
                UserId = requesterId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByRequesterIdAsync(requesterId))
                .ReturnsAsync(new List<FriendReqResponse>());

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.FindFriendReqByRequesterIdAsync(requesterId);

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task FindFriendReqByRequesterIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int requesterId = 1;

            var userResponse = new UserResponse
            {
                UserId = requesterId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByRequesterIdAsync(requesterId))
                .ThrowsAsync(new Exception("Exception"));

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.FindFriendReqByRequesterIdAsync(requesterId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task FindFriendReqByReceiverIdAsync_ShouldReturnStatusCode200_WhenRequestsExist()
        {
            // Arrange
            int receiverId = 2;

            var userResponse = new UserResponse
            {
                UserId = receiverId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            var friendReqs = new List<FriendReqResponse>
            {
                new FriendReqResponse
                {
                    FriendRequestId = 1,
                    RequesterId = 1,
                    ReceiverId = receiverId,
                    Status = FriendReqStatus.Pending
                }
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByReceiverIdAsync(receiverId))
                .ReturnsAsync(friendReqs);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.FindFriendReqByReceiverIdAsync(receiverId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task FindFriendReqByReceiverIdAsync_ShouldReturnStatusCode204_WhenNoRequestsExist()
        {
            // Arrange
            int receiverId = 2;

            var userResponse = new UserResponse
            {
                UserId = receiverId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByReceiverIdAsync(receiverId))
                .ReturnsAsync(new List<FriendReqResponse>());

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.FindFriendReqByReceiverIdAsync(receiverId);

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task FindFriendReqByReceiverIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int receiverId = 2;

            var userResponse = new UserResponse
            {
                UserId = receiverId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByReceiverIdAsync(receiverId))
                .ThrowsAsync(new Exception("Exception"));

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.FindFriendReqByReceiverIdAsync(receiverId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task CreateFriendReqAsync_ShouldReturnStatusCode200_WhenCreatedSuccessfully()
        {
            // Arrange
            var userResponse = new UserResponse
            {
                UserId = 1,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            var request = new FriendReqRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            var response = new FriendReqResponse
            {
                FriendRequestId = 1,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Pending
            };

            _friendReqServiceMock
                .Setup(x => x.CreateFriendReqAsync(request))
                .ReturnsAsync(response);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.CreateFriendReqAsync(request);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateFriendReqAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var userResponse = new UserResponse
            {
                UserId = 1,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            var request = new FriendReqRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            _friendReqServiceMock
                .Setup(x => x.CreateFriendReqAsync(request))
                .ThrowsAsync(new Exception("Exception"));

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.CreateFriendReqAsync(request);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task UpdateFriendReqByIdAsync_ShouldReturnStatusCode200_WhenUpdatedSuccessfully()
        {
            // Arrange
            int friendRequestId = 2;

            var updateRequest = new FriendReqUpdateRequest
            {
                Status = FriendReqStatus.Accepted
            };

            var currentFriendReq = new FriendReqResponse
            {
                FriendRequestId = friendRequestId,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Pending
            };

            var updatedResponse = new FriendReqResponse
            {
                FriendRequestId = friendRequestId,
                RequesterId = 1,
                ReceiverId = 2,
                Status = FriendReqStatus.Accepted
            };

            var userResponse = new UserResponse
            {
                UserId = 1,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _friendReqServiceMock
                .Setup(x => x.FindFriendReqByIdAsync(friendRequestId))
                .ReturnsAsync(currentFriendReq);

            _friendReqServiceMock
                .Setup(x => x.UpdateFriendReqByIdAsync(friendRequestId, updateRequest))
                .ReturnsAsync(updatedResponse);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.UpdateFriendReqByIdAsync(friendRequestId, updateRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateFriendReqByIdAsync_ShouldReturnStatusCode404_WhenRequestNotFound()
        {
            // Arrange
            int reqId = 1;

            var request = new FriendReqUpdateRequest
            {
                Status = FriendReqStatus.Accepted
            };

            _friendReqServiceMock
                .Setup(x => x.UpdateFriendReqByIdAsync(reqId, request))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _friendReqController.UpdateFriendReqByIdAsync(reqId, request);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }
    }
}
