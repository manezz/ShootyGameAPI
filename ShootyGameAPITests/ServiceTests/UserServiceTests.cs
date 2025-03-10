using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IJwtUtils> _jwtUtilsMock = new();
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

        public UserServiceTests()
        {
            _userService = new UserService(
                _userRepositoryMock.Object,
                _jwtUtilsMock.Object,
                _passwordHasherMock.Object,
                _httpContextAccessor.Object);
        }

        [Fact]
        public async Task FindAllAsync_ShouldReturnListOfUserResponses_WhenUsersExist()
        {
            // Arrange
            List<User> users = new()
            {
                new()
                {
                    UserId = 1,
                    UserName = "TestUser",
                    Email = "admin@mail.dk",
                    Role = Role.Admin,
                },
                new()
                {
                    UserId = 2,
                    UserName = "TestUser",
                    Email = "user@mail.dk",
                    Role = Role.User
                },
            };

            _userRepositoryMock
                .Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.FindAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserResponse>>(result);
            Assert.Equal(2, result?.Count);
        }

        [Fact]
        public async Task FindAllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            List<User> users = new();

            _userRepositoryMock
                .Setup(x => x.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.FindAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnUserResponse_WhenUserExists()
        {
            // Arrange
            int userId = 1;

            User user = new()
            {
                UserId = userId,
                UserName = "TestUser",
                Email = "admin@mail.dk",
                Role = Role.Admin
            };

            _userRepositoryMock
                .Setup(x => x.FindUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.FindUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(user.UserId, result?.UserId);
            Assert.Equal(user.UserName, result?.UserName);
        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.FindUserByIdAsync(It.IsAny<int>()))
                    .ReturnsAsync(() => null);

            // Act
            var result = await _userService.FindUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnUserResponse_WhenCreateIsSuccess()
        {
            // Arrange
            UserRequest newUser = new()
            {
                UserName = "TestUser",
                Email = "admin@mail.dk",
                Password = "Passw0rd",
                Role = Role.User
            };
            int userId = 1;

            User user = new()
            {
                UserId = userId,
                UserName = "TestUser",
                Email = "user@mail.dk",
                Role = Role.User,
            };

            _userRepositoryMock
                .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashedPassword");

            // Act
            var result = await _userService.CreateUserAsync(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(user.UserId, result?.UserId);
            Assert.Equal(user.UserName, result?.UserName);
            Assert.Equal(user.Email, result?.Email);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            UserRequest newUser = new()
            {
                UserName = "TestUser",
                Email = "admin@mail.dk",
                Password = "Passw0rd",
                Role = Role.User
            };

            _userRepositoryMock
                .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(() => null!);

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("hashedPassword");

            // Act
            var result = await _userService.CreateUserAsync(newUser);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUserResponse_WhenUpdateIsSuccess()
        {
            // Arrange
            UserRequest updateUser = new()
            {
                UserName = "UpdatedUser",
                Email = "updated@mail.dk",
                Password = "newPassword",
                Role = Role.User
            };
            int userId = 1;

            User updatedUserEntity = new()
            {
                UserId = userId,
                UserName = updateUser.UserName,
                Email = updateUser.Email,
                Role = updateUser.Role
            };

            _userRepositoryMock
                .Setup(x => x.UpdateUserByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(updatedUserEntity);

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("newHashedPassword");

            // Act
            var result = await _userService.UpdateUserByIdAsync(userId, updateUser);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(userId, result?.UserId);
            Assert.Equal(updateUser.UserName, result?.UserName);
            Assert.Equal(updateUser.Email, result?.Email);
            Assert.Equal(updateUser.Role, result?.Role);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            UserRequest updateUser = new()
            {
                UserName = "UpdatedUser",
                Email = "updated@mail.dk",
                Password = "newPassword",
                Role = Role.User
            };
            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.UpdateUserByIdAsync(It.IsAny<int>(), It.IsAny<User>()))
                .ReturnsAsync(() => null);

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("newHashedPassword");

            // Act
            var result = await _userService.UpdateUserByIdAsync(userId, updateUser);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task DeleteAsync_ShouldReturnUserResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            int userId = 1;

            User user = new()
            {
                UserId = userId,
                UserName = "TestUser",
                Email = "admin@mail.dk",
                Role = Role.Admin
            };

            _userRepositoryMock
                .Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.DeleteUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(user.UserId, result?.UserId);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _userRepositoryMock
                .Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userService.DeleteUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }
    }
}
