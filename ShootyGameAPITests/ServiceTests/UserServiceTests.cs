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
        private readonly Mock<IUserWeaponRepository> _userWeaponRepositoryMock = new();
        private readonly Mock<IWeaponRepository> _weaponRepositoryMock = new();
        private readonly Mock<IFriendRepository> _friendRepositoryMock = new();
        private readonly Mock<IJwtUtils> _jwtUtilsMock = new();
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

        public UserServiceTests()
        {
            _userService = new UserService(
                _userRepositoryMock.Object,
                _userWeaponRepositoryMock.Object,
                _friendRepositoryMock.Object,
                _weaponRepositoryMock.Object,
                _jwtUtilsMock.Object,
                _passwordHasherMock.Object,
                _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnSignInResponse_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new SignInRequest
            {
                Email = "admin@mail.dk",
                Password = "ValidPassword"
            };

            var user = new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "admin@mail.dk",
                PasswordHash = "hashedPassword",
                Role = Role.Admin,
            };

            _userRepositoryMock
                .Setup(x => x.FindUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            _jwtUtilsMock
                .Setup(x => x.GenerateJwtToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            // Act
            var result = await _userService.AuthenticateAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SignInResponse>(result);
            Assert.Equal(user.UserId, result?.UserId);
            Assert.Equal("fake-jwt-token", result?.Token);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var request = new SignInRequest
            {
                Email = "admin@mail.dk",
                Password = "InvalidPassword"
            };

            _userRepositoryMock
                .Setup(x => x.FindUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _userService.AuthenticateAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddWeaponToUserAsync_ShouldReturnUserResponse_WhenUserExists()
        {
            // Arrange
            var userWeaponRequest = new UserWeaponRequest
            {
                UserId = 1,
                WeaponId = 2
            };

            Weapon weapon = new Weapon
            {
                WeaponId = 2,
                Name = "TestWeapon",
                Price = 100,
                ReloadSpeed = 1.5f,
                MagSize = 30,
                FireRate = 600,
                FireMode = FireMode.Auto,
                WeaponType = new()
            };

            var user = new User
            {
                UserId = 1,
                UserName = "TestUser",
                Email = "user@mail.dk",
                Role = Role.User,
                Money = 1000,
                UserWeapons = new(),
                Scores = new(),
                FriendsAsRequester = new(),
                FriendsAsReceiver = new(),
                SentFriendReqs = new(),
                ReceivedFriendReqs = new()
            };

            var userWeapon = new UserWeapon
            {
                UserId = 1,
                WeaponId = 2
            };

            _userRepositoryMock
                .Setup(x => x.FindUserByIdAsync(userWeaponRequest.UserId))
                .ReturnsAsync(user);

            _weaponRepositoryMock
                .Setup(x => x.FindWeaponByIdAsync(userWeaponRequest.WeaponId))
                .ReturnsAsync(weapon);

            _userWeaponRepositoryMock
                .Setup(x => x.CreateUserWeaponAsync(It.IsAny<UserWeapon>()))
                .ReturnsAsync(userWeapon);

            // Act
            var result = await _userService.AddWeaponToUserAsync(userWeaponRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.UserId, result?.UserId);
        }

        [Fact]
        public async Task AddWeaponToUserAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userWeaponRequest = new UserWeaponRequest
            {
                UserId = 1,
                WeaponId = 2
            };

            _userWeaponRepositoryMock
                .Setup(x => x.CreateUserWeaponAsync(It.IsAny<UserWeapon>()))
                .ReturnsAsync((UserWeapon)null!);

            // Act
            var result = await _userService.AddWeaponToUserAsync(userWeaponRequest);

            // Assert
            Assert.Null(result);
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
                    UserWeapons = new(),
                    Scores = new(),
                    FriendsAsRequester = new(),
                    FriendsAsReceiver = new(),
                    SentFriendReqs = new(),
                    ReceivedFriendReqs = new()
                },
                new()
                {
                    UserId = 2,
                    UserName = "TestUser",
                    Email = "user@mail.dk",
                    Role = Role.User,
                    UserWeapons = new(),
                    Scores = new(),
                    FriendsAsRequester = new(),
                    FriendsAsReceiver = new(),
                    SentFriendReqs = new(),
                    ReceivedFriendReqs = new()
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
                Role = Role.Admin,
                UserWeapons = new(),
                Scores = new(),
                FriendsAsRequester = new(),
                FriendsAsReceiver = new(),
                SentFriendReqs = new(),
                ReceivedFriendReqs = new()
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
                UserWeapons = new(),
                Scores = new(),
                FriendsAsRequester = new(),
                FriendsAsReceiver = new(),
                SentFriendReqs = new(),
                ReceivedFriendReqs = new()
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
                Role = updateUser.Role,
                UserWeapons = new(),
                Scores = new(),
                FriendsAsRequester = new(),
                FriendsAsReceiver = new(),
                SentFriendReqs = new(),
                ReceivedFriendReqs = new()
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
                Role = Role.Admin,
                UserWeapons = new(),
                Scores = new(),
                FriendsAsRequester = new(),
                FriendsAsReceiver = new(),
                SentFriendReqs = new(),
                ReceivedFriendReqs = new()
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
