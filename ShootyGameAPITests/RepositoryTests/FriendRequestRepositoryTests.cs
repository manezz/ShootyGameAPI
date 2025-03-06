using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class FriendRequestRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly FriendRequestRepository _friendRequestRepository;

        public FriendRequestRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "FriendRequestRepositoryTests")
                .Options;

            _context = new(_options);
            _friendRequestRepository = new(_context);
        }

        [Fact]
        public async Task FindAllFriendRequestsByRequesterIdAsync_ShouldReturnListOfFriendRequests_WhenFriendRequestsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                PlayerTag = "TestUser#1",
                Email = "admin@mail.com",
                PasswordHash = "Passw0rd"
            });
            _context.Users.Add(new User
            {
                UserId = 2,
                UserName = "TestUser",
                PlayerTag = "TestUser#2",
                Email = "user1@mail.com",
                PasswordHash = "Passw0rd"
            });
            _context.Users.Add(new User
            {
                UserId = 3,
                UserName = "TestUser",
                PlayerTag = "TestUser#3",
                Email = "user2@mail.com",
                PasswordHash = "Passw0rd"
            });

            _context.FriendRequests.Add(new FriendRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            });
            _context.FriendRequests.Add(new FriendRequest
            {
                RequesterId = 1,
                ReceiverId = 3
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRequestRepository.FindAllFriendRequestsByRequesterIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendRequest>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindAllFriendRequestsByRequesterIdAsync_ShouldReturnEmptyList_WhenNoFriendRequestsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRequestRepository.FindAllFriendRequestsByRequesterIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendRequest>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindAllFriendRequestsByReceiverIdAsync_ShouldReturnListOfFriendRequests_WhenFriendRequestsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                PlayerTag = "TestUser#1",
                Email = "admin@mail.com",
                PasswordHash = "Passw0rd"
            });
            _context.Users.Add(new User
            {
                UserId = 2,
                UserName = "TestUser",
                PlayerTag = "TestUser#2",
                Email = "user1@mail.com",
                PasswordHash = "Passw0rd"
            });
            _context.Users.Add(new User
            {
                UserId = 3,
                UserName = "TestUser",
                PlayerTag = "TestUser#3",
                Email = "user2@mail.com",
                PasswordHash = "Passw0rd"
            });

            _context.FriendRequests.Add(new FriendRequest
            {
                RequesterId = 2,
                ReceiverId = 1
            });
            _context.FriendRequests.Add(new FriendRequest
            {
                RequesterId = 3,
                ReceiverId = 1
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRequestRepository.FindAllFriendRequestsByReceiverIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendRequest>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindAllFriendRequestsByReceiverIdAsync_ShouldReturnEmptyList_WhenNoFriendRequestsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRequestRepository.FindAllFriendRequestsByReceiverIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendRequest>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindFriendRequestByIdAsync_ShouldReturnFriendRequest_WhenFriendRequestExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            var friendRequest = new FriendRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };
            _context.FriendRequests.Add(friendRequest);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRequestRepository.FindFriendRequestByIdAsync(friendRequest.FriendRequestId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(friendRequest.FriendRequestId, result.FriendRequestId);
        }

        [Fact]
        public async Task FindFriendRequestByIdAsync_ShouldReturnNull_WhenFriendRequestDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRequestRepository.FindFriendRequestByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateFriendRequestAsync_ShouldAddNewId_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var newFriendRequest = new FriendRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            // Act
            var result = await _friendRequestRepository.CreateFriendRequestAsync(newFriendRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FriendRequest>(result);
            Assert.NotEqual(0, result.FriendRequestId); // Check that a new ID is assigned
        }

        [Fact]
        public async Task CreateFriendRequestAsync_ShouldFailToAddDuplicateFriendRequest_WhenFriendRequestIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var friendRequest = new FriendRequest
            {
                FriendRequestId = 1,
                RequesterId = 1,
                ReceiverId = 2
            };

            await _friendRequestRepository.CreateFriendRequestAsync(friendRequest);

            // Act
            async Task action() => await _friendRequestRepository.CreateFriendRequestAsync(friendRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task DeleteFriendRequestByIdAsync_ShouldRemoveFriendRequestFromDatabase_WhenFriendRequestExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();
            var friendRequest = new FriendRequest
            {
                RequesterId = 1,
                ReceiverId = 2
            };
            _context.FriendRequests.Add(friendRequest);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRequestRepository.DeleteFriendRequestByIdAsync(friendRequest.FriendRequestId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(friendRequest.FriendRequestId, result.FriendRequestId);
            Assert.Null(await _context.FriendRequests.FindAsync(friendRequest.FriendRequestId));
        }

        [Fact]
        public async Task DeleteFriendRequestByIdAsync_ShouldReturnNull_WhenFriendRequestDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRequestRepository.DeleteFriendRequestByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }

}
