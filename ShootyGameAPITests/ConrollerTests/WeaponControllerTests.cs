using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using ShootyGameAPI.Controllers;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ConrollerTests
{
    public class WeaponControllerTests
    {
        private readonly WeaponController _weaponController;
        private readonly Mock<IWeaponService> _weaponServiceMock = new();
        private readonly HttpContext httpContext = new DefaultHttpContext();

        public WeaponControllerTests()
        {
            _weaponController = new WeaponController(_weaponServiceMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task GetAllWeaponsAsync_ShouldReturnStatusCode200_WhenWeaponsExist()
        {
            // Arrange
            var weaponResponses = new List<WeaponResponse>
        {
            new WeaponResponse
            {
                WeaponId = 1,
                Name = "Rifle"
            },
            new WeaponResponse
            {
                WeaponId = 2,
                Name = "Shotgun"
            }
        };

            _weaponServiceMock
                .Setup(x => x.GetAllWeaponsAsync())
                .ReturnsAsync(weaponResponses);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.GetAllWeaponsAsync();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllWeaponsAsync_ShouldReturnStatusCode204_WhenNoWeaponsExist()
        {
            // Arrange
            var weaponResponses = new List<WeaponResponse>();

            _weaponServiceMock
                .Setup(x => x.GetAllWeaponsAsync())
                .ReturnsAsync(weaponResponses);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.GetAllWeaponsAsync();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task GetAllWeaponsAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _weaponServiceMock
                .Setup(x => x.GetAllWeaponsAsync())
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.GetAllWeaponsAsync();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnStatusCode200_WhenWeaponExists()
        {
            // Arrange
            int weaponId = 1;
            var weaponResponse = new WeaponResponse
            {
                WeaponId = weaponId,
                Name = "Rifle"
            };

            _weaponServiceMock
                .Setup(x => x.FindWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weaponResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.FindWeaponByIdAsync(weaponId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnStatusCode404_WhenWeaponDoesNotExist()
        {
            // Arrange
            int weaponId = 1;

            _weaponServiceMock
                .Setup(x => x.FindWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.FindWeaponByIdAsync(weaponId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int weaponId = 1;

            _weaponServiceMock
                .Setup(x => x.FindWeaponByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.FindWeaponByIdAsync(weaponId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task CreateWeaponAsync_ShouldReturnStatusCode200_WhenWeaponIsSuccessfullyCreated()
        {
            // Arrange
            var weaponRequest = new WeaponRequest
            {
                Name = "Rifle"
            };

            var weaponResponse = new WeaponResponse
            {
                WeaponId = 1,
                Name = "Rifle"
            };

            _weaponServiceMock
                .Setup(x => x.CreateWeaponAsync(It.IsAny<WeaponRequest>()))
                .ReturnsAsync(weaponResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.CreateWeaponAsync(weaponRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateWeaponAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var weaponRequest = new WeaponRequest
            {
                Name = "Rifle"
            };

            _weaponServiceMock
                .Setup(x => x.CreateWeaponAsync(It.IsAny<WeaponRequest>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.CreateWeaponAsync(weaponRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task UpdateWeaponByIdAsync_ShouldReturnStatusCode200_WhenWeaponIsUpdated()
        {
            // Arrange
            int weaponId = 1;
            var weaponRequest = new WeaponRequest
            {
                Name = "Updated Rifle"
            };

            var weaponResponse = new WeaponResponse
            {
                WeaponId = weaponId,
                Name = "Updated Rifle"
            };

            _weaponServiceMock
                .Setup(x => x.UpdateWeaponByIdAsync(It.IsAny<int>(), It.IsAny<WeaponRequest>()))
                .ReturnsAsync(weaponResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.UpdateWeaponByIdAsync(weaponId, weaponRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateWeaponByIdAsync_ShouldReturnStatusCode404_WhenWeaponDoesNotExist()
        {
            // Arrange
            int weaponId = 1;
            var weaponRequest = new WeaponRequest
            {
                Name = "Updated Rifle"
            };

            _weaponServiceMock
                .Setup(x => x.UpdateWeaponByIdAsync(It.IsAny<int>(), It.IsAny<WeaponRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.UpdateWeaponByIdAsync(weaponId, weaponRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnStatusCode200_WhenWeaponIsDeleted()
        {
            // Arrange
            int weaponId = 1;
            var weaponResponse = new WeaponResponse
            {
                WeaponId = weaponId,
                Name = "Rifle"
            };

            _weaponServiceMock
                .Setup(x => x.DeleteWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weaponResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.DeleteWeaponByIdAsync(weaponId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnStatusCode404_WhenWeaponDoesNotExist()
        {
            // Arrange
            int weaponId = 1;

            _weaponServiceMock
                .Setup(x => x.DeleteWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.DeleteWeaponByIdAsync(weaponId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int weaponId = 1;

            _weaponServiceMock
                .Setup(x => x.DeleteWeaponByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponController.DeleteWeaponByIdAsync(weaponId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
