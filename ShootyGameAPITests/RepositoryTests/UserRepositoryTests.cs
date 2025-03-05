using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class UserRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "UserRepositoryTests")
                .Options;

            _context = new(_options);
            _userRepository = new(_context);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnListOfUsers_WhenUsersExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            });
            _context.Users.Add(new User
            {
                UserId = 2,
                UserName = "TestUser",
                Email = "user@mail.com",
                PlayerTag = "TestUser#2",
                PasswordHash = "Passw0rd"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnEmptyListOfUsers_WhenNoUsersExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userRepository.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindUserByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.FindUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(1, result?.UserId);
        }

        [Fact]
        public async Task FindUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userRepository.FindUserByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task FindUserByEmailAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.FindUserByEmailAsync("admin@mail.com");

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal("admin@mail.com", result?.Email);
        }

        [Fact]
        public async Task FindUserByEmailAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userRepository.FindUserByEmailAsync("admin@mail.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldAddNewId_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;

            User newUser = new()
            {
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            };

            // Act
            var result = await _userRepository.CreateUserAsync(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(expectedNewId, result?.UserId);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldFailToAddNewUser_WhenUserIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            User user = new()
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            };

            await _userRepository.CreateUserAsync(user);

            // Act
            async Task action() => await _userRepository.CreateUserAsync(user);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_ShouldChangeValuesOnUser_WhenUserExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var user = new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            User updatedUser = new()
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "updated@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "NewPassw0rd"
            };

            // Act
            var result = await _userRepository.UpdateUserByIdAsync(1, updatedUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(updatedUser.Email, result?.Email);
            Assert.Equal(updatedUser.UserName, result?.UserName);
            Assert.Equal(updatedUser.PlayerTag, result?.PlayerTag);
            Assert.Equal(updatedUser.PasswordHash, result?.PasswordHash);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var updatedUser = new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            };

            // Act
            var result = await _userRepository.UpdateUserByIdAsync(1, updatedUser);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnDeletedUser_WhenUserIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var user = new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.com",
                PlayerTag = "TestUser#1",
                PasswordHash = "Passw0rd"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userRepository.DeleteUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(1, result.UserId);
        }

        [Fact]
        public async Task DeleteByUserIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userRepository.DeleteUserByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
