using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class FriendRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly FriendRepository _friendRepository;

        public FriendRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "FriendRepositoryTests")
                .Options;

            _context = new(_options);
            _friendRepository = new(_context);
        }

        [Fact]
        public async Task FindAllFriendsByUserIdAsync_ShouldReturnListOfFriends_WhenFriendsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                PasswordHash = "Passw0rd",
                PlayerTag = "TestUser#1",
                Email = "admin@mail.con"
            });
            _context.Users.Add(new User
            {
                UserId = 2,
                UserName = "TestUser",
                PasswordHash = "Passw0rd",
                PlayerTag = "TestUser#2",
                Email = "User1@mail.com"
            });
            _context.Users.Add(new User
            {
                UserId = 3,
                UserName = "TestUser",
                PasswordHash = "Passw0rd",
                PlayerTag = "TestUser#3",
                Email = "User2@mail.com"
            });

            _context.Friends.Add(new Friend
            {
                User1Id = 1,
                User2Id = 2
            });
            _context.Friends.Add(new Friend
            {
                User1Id = 1,
                User2Id = 3
            });
            _context.Friends.Add(new Friend
            {
                User1Id = 2,
                User2Id = 3
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRepository.FindAllFriendsByUserIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Friend>>(result);
            Assert.Equal(2, result.Count); // User 1 has 2 friends
        }

        [Fact]
        public async Task FindAllFriendsByUserIdAsync_ShouldReturnEmptyList_WhenUserHasNoFriends()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRepository.FindAllFriendsByUserIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Friend>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindFriendByIdAsync_ShouldReturnFriend_WhenFriendshipExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Friends.Add(new Friend
            {
                User1Id = 1,
                User2Id = 2
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRepository.FindFriendByIdAsync(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Friend>(result);
            Assert.Equal(1, result?.User1Id);
            Assert.Equal(2, result?.User2Id);
        }

        [Fact]
        public async Task FindFriendByIdAsync_ShouldReturnNull_WhenFriendshipDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRepository.FindFriendByIdAsync(1, 2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateFriendAsync_ShouldAddNewFriend_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Friend newFriend = new()
            {
                User1Id = 1,
                User2Id = 2
            };

            // Act
            var result = await _friendRepository.CreateFriendAsync(newFriend);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Friend>(result);
            Assert.Equal(1, result.User1Id);
            Assert.Equal(2, result.User2Id);
        }

        [Fact]
        public async Task CreateFriendAsync_ShouldFailToAddDuplicateFriend_WhenFriendshipAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Friend existingFriend = new()
            {
                User1Id = 1,
                User2Id = 2
            };

            await _friendRepository.CreateFriendAsync(existingFriend);

            // Act
            async Task action() => await _friendRepository.CreateFriendAsync(existingFriend);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task DeleteFriendByIdAsync_ShouldReturnDeletedFriend_WhenFriendIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var friend = new Friend
            {
                User1Id = 1,
                User2Id = 2
            };

            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendRepository.DeleteFriendByIdAsync(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Friend>(result);
            Assert.Equal(1, result.User1Id);
            Assert.Equal(2, result.User2Id);
        }

        [Fact]
        public async Task DeleteFriendByIdAsync_ShouldReturnNull_WhenFriendshipDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _friendRepository.DeleteFriendByIdAsync(1, 2);

            // Assert
            Assert.Null(result);
        }
    }

}
