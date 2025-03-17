using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class FriendReqRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly FriendRequestRepository _friendReqRepository;

        public FriendReqRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "FriendReqRepositoryTests")
                .Options;

            _context = new(_options);
            _friendReqRepository = new(_context);
        }

        [Fact]
        public async Task FindAllFriendReqByRequesterIdAsync_ShouldReturnListOfFriendReqs_WhenFriendReqsExist()
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

            _context.FriendReqs.Add(new FriendReq
            {
                RequesterId = 1,
                ReceiverId = 2
            });
            _context.FriendReqs.Add(new FriendReq
            {
                RequesterId = 1,
                ReceiverId = 3
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendReqRepository.FindAllFriendReqByRequesterIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendReq>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindAllFriendReqsByRequesterIdAsync_ShouldReturnEmptyList_WhenNoFriendReqsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendReqRepository.FindAllFriendReqByRequesterIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendReq>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindAllFriendReqsByReceiverIdAsync_ShouldReturnListOfFriendReqs_WhenFriendReqsExist()
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

            _context.FriendReqs.Add(new FriendReq
            {
                RequesterId = 2,
                ReceiverId = 1
            });
            _context.FriendReqs.Add(new FriendReq
            {
                RequesterId = 3,
                ReceiverId = 1
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendReqRepository.FindAllFriendReqByReceiverIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendReq>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindAllFriendReqsByReceiverIdAsync_ShouldReturnEmptyList_WhenNoFriendReqsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendReqRepository.FindAllFriendReqByReceiverIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<FriendReq>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindFriendRequestByIdAsync_ShouldReturnFriendRequest_WhenFriendRequestExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                },
                new User
                {
                    UserId = 2,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#2",
                    Email = "user@mail.com",
                    PasswordHash = "Passw0rd"
                }
            };

            var friendReq = new FriendReq
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            _context.Users.AddRange(users);
            _context.FriendReqs.Add(friendReq);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendReqRepository.FindFriendReqByIdAsync(friendReq.FriendReqId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(friendReq.FriendReqId, result.FriendReqId);
        }

        [Fact]
        public async Task FindFriendReqByIdAsync_ShouldReturnNull_WhenFriendReqDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendReqRepository.FindFriendReqByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateFriendReqAsync_ShouldAddNewId_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                },
                new User
                {
                    UserId = 2,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#2",
                    Email = "user@mail.com",
                    PasswordHash = "Passw0rd"
                }
            };

            _context.Users.AddRange(users);
            await _context.SaveChangesAsync();

            var newFriendRequest = new FriendReq
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            // Act
            var result = await _friendReqRepository.CreateFriendReqAsync(newFriendRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FriendReq>(result);
            Assert.NotEqual(0, result.FriendReqId); // Check that a new ID is assigned
        }

        [Fact]
        public async Task CreateFriendReqAsync_ShouldFailToAddDuplicateFriendReq_WhenFriendReqIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var friendRequest = new FriendReq
            {
                FriendReqId = 1,
                RequesterId = 1,
                ReceiverId = 2
            };

            await _friendReqRepository.CreateFriendReqAsync(friendRequest);

            // Act
            async Task action() => await _friendReqRepository.CreateFriendReqAsync(friendRequest);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task DeleteFriendReqByIdAsync_ShouldReturnFriendReqDeleted_WhenFriendReqIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                },
                new User
                {
                    UserId = 2,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#2",
                    Email = "user@mail.com",
                    PasswordHash = "Passw0rd"
                }
            };

            var FriendReq = new FriendReq
            {
                RequesterId = 1,
                ReceiverId = 2
            };

            _context.Users.AddRange(users);
            _context.FriendReqs.Add(FriendReq);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendReqRepository.DeleteFriendReqByIdAsync(FriendReq.FriendReqId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FriendReq>(result);
            Assert.Equal(FriendReq.FriendReqId, result.FriendReqId);
        }

        [Fact]
        public async Task DeleteFriendReqByIdAsync_ShouldReturnNull_WhenFriendReqDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendReqRepository.DeleteFriendReqByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }

}
