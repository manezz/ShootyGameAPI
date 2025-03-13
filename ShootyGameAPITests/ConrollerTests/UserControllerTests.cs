using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using ShootyGameAPI.Controllers;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ConrollerTests
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly HttpContext httpContext = new DefaultHttpContext();

        public UserControllerTests()
        {
            _userController = new UserController(_userServiceMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnStatusCode200_WhenUserExists()
        {
            // Arrange
            var signInRequest = new SignInRequest
            {
                Email = "admin@mail.com",
                Password = "Passw0rd"
            };

            var signInResponse = new SignInResponse
            {
                UserId = 1,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _userServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<SignInRequest>()))
                .ReturnsAsync(signInResponse);

            // Act
            var result = (IStatusCodeActionResult)await _userController.AuthenticateAsync(signInRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            var signInRequest = new SignInRequest
            {
                Email = "admin@mail.com",
                Password = "Passw0rd"
            };

            _userServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<SignInRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _userController.AuthenticateAsync(signInRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var signInRequest = new SignInRequest
            {
                Email = "admin@mail.com",
                Password = "Passw0rd"
            };

            _userServiceMock
                .Setup(x => x.AuthenticateAsync(It.IsAny<SignInRequest>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _userController.AuthenticateAsync(signInRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task AddWeaponToUserAsync_ShouldReturnStatusCode200_WhenWeaponIsAddedToUser()
        {
            // Arrange
            int userId = 1;
            var userWeaponRequest = new UserWeaponRequest
            {
                WeaponId = 1,
                UserId = userId
            };

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            var userResponseWithWeapon = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin,
                Weapons = new List<User_WeaponsResponse>
                {
                    new User_WeaponsResponse
                    {
                        WeaponId = 1,
                        Name = "Pistol"
                    }
                }
            };

            _userServiceMock
                .Setup(x => x.AddWeaponToUserAsync(It.IsAny<UserWeaponRequest>()))
                .ReturnsAsync(userResponseWithWeapon);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.AddWeaponToUserAsync(userId, userWeaponRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AddWeaponToUserAsync_ShouldReturnStatusCode401_WhenUnauthorized()
        {
            // Arrange
            int userId = 1;
            var userWeaponRequest = new UserWeaponRequest
            {
                WeaponId = 1,
                UserId = userId
            };

            var userResponse = new UserResponse
            {
                UserId = 2,
                Email = "user@mail.com",
                Role = Role.User
            };

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.AddWeaponToUserAsync(userId, userWeaponRequest);

            // Assert
            Assert.Equal(401, result.StatusCode);
        }

        [Fact]
        public async Task AddWeaponToUserAsync_ShouldReturnStatusCode404_WhenUserNotFound()
        {
            // Arrange
            int userId = 1;
            var userWeaponRequest = new UserWeaponRequest
            {
                WeaponId = 1,
                UserId = userId
            };

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _userServiceMock
                .Setup(x => x.AddWeaponToUserAsync(It.IsAny<UserWeaponRequest>()))
                .ReturnsAsync(() => null);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.AddWeaponToUserAsync(userId, userWeaponRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task AddWeaponToUserAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int userId = 1;
            var userWeaponRequest = new UserWeaponRequest
            {
                WeaponId = 1,
                UserId = userId
            };

            _userServiceMock
                .Setup(x => x.AddWeaponToUserAsync(It.IsAny<UserWeaponRequest>()))
                .ThrowsAsync(new Exception("This is an exception"));

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.AddWeaponToUserAsync(userId, userWeaponRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }


        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnStatusCode200_WhenUsersExist()
        {
            // Arrange
            var userResponses = new List<UserResponse>
            {
                new UserResponse
                {
                    UserId = 1,
                    Email = "admin@mail.com",
                    Role = Role.Admin
                },
                new UserResponse
                {
                    UserId = 2,
                    Email = "user@mail.com",
                    Role = Role.User
                }
            };

            _userServiceMock
                .Setup(x => x.FindAllUsersAsync())
                .ReturnsAsync(userResponses);

            // Act
            var result = (IStatusCodeActionResult)await _userController.GetAllUsersAsync();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnStatusCode204_WhenNoUsersExist()
        {
            // Arrange
            var userResponses = new List<UserResponse>();

            _userServiceMock
                .Setup(x => x.FindAllUsersAsync())
                .ReturnsAsync(userResponses);

            // Act
            var result = (IStatusCodeActionResult)await _userController.GetAllUsersAsync();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _userServiceMock
                .Setup(x => x.FindAllUsersAsync())
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _userController.GetAllUsersAsync();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task FindUserByIdAsync_ShouldReturnStatusCode200_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _userServiceMock
                .Setup(x => x.FindUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(userResponse);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.FindUserByIdAsync(userId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task FindUserByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            httpContext.Items["User"] = userResponse;

            _userServiceMock
                .Setup(x => x.FindUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _userController.FindUserByIdAsync(userId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task FindUserByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int userId = 1;

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            httpContext.Items["User"] = userResponse;

            _userServiceMock
                .Setup(x => x.FindUserByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _userController.FindUserByIdAsync(userId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnStatusCode200_WhenUserIsSuccessfullyCreated()
        {
            // Arrange
            var userRequest = new UserRequest
            {
                Email = "admin@mail.com",
                Password = "Passw0rd"
            };

            var userResponse = new UserResponse
            {
                UserId = 1,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _userServiceMock
                .Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>()))
                .ReturnsAsync(userResponse);

            // Act
            var result = (IStatusCodeActionResult)await _userController.CreateUserAsync(userRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var userRequest = new UserRequest
            {
                Email = "admin@mail.com",
                Password = "Passw0rd"
            };

            _userServiceMock
                .Setup(x => x.CreateUserAsync(It.IsAny<UserRequest>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _userController.CreateUserAsync(userRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_ShouldReturnStatusCode200_WhenUserIsUpdated()
        {
            // Arrange
            int userId = 1;
            var userRequest = new UserRequest
            {
                Email = "updated@mail.com",
                Password = "newpassword"
            };
            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _userServiceMock
                .Setup(x => x.UpdateUserByIdAsync(It.IsAny<int>(), It.IsAny<UserRequest>()))
                .ReturnsAsync(userResponse);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.UpdateUserByIdAsync(userId, userRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateUserByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            var userRequest = new UserRequest
            {
                Email = "updated@mail.com",
                Password = "newpassword"
            };

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            httpContext.Items["User"] = userResponse;

            _userServiceMock
                .Setup(x => x.UpdateUserByIdAsync(It.IsAny<int>(), It.IsAny<UserRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _userController.UpdateUserByIdAsync(userId, userRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnStatusCode200_WhenUserIsDeleted()
        {
            // Arrange
            int userId = 1;
            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            _userServiceMock
                .Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(userResponse);

            httpContext.Items["User"] = userResponse;

            // Act
            var result = (IStatusCodeActionResult)await _userController.DeleteUserByIdAsync(userId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnStatusCode404_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            httpContext.Items["User"] = userResponse;

            _userServiceMock
                .Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _userController.DeleteUserByIdAsync(userId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteUserByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int userId = 1;

            var userResponse = new UserResponse
            {
                UserId = userId,
                Email = "admin@mail.com",
                Role = Role.Admin
            };

            httpContext.Items["User"] = userResponse;

            _userServiceMock
                .Setup(x => x.DeleteUserByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _userController.DeleteUserByIdAsync(userId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }

}
