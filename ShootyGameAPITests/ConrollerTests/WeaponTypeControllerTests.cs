using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using ShootyGameAPI.Controllers;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ConrollerTests
{
    public class WeaponTypeControllerTests
    {
        private readonly WeaponTypeController _weaponTypeController;
        private readonly Mock<IWeaponTypeService> _weaponTypeServiceMock = new();
        private readonly HttpContext httpContext = new DefaultHttpContext();

        public WeaponTypeControllerTests()
        {
            _weaponTypeController = new WeaponTypeController(_weaponTypeServiceMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnStatusCode200_WhenWeaponTypesExist()
        {
            // Arrange
            var weaponTypeResponses = new List<WeaponTypeResponse>
            {
                new WeaponTypeResponse
                {
                    WeaponTypeId = 1,
                    Name = "Rifle",
                    EquipmentSlot = EquipmentSlot.Primary
                },
                new WeaponTypeResponse
                {
                    WeaponTypeId = 2,
                    Name = "Shotgun",
                    EquipmentSlot = EquipmentSlot.Secondary
                }
            };

            _weaponTypeServiceMock
                .Setup(x => x.GetAllWeaponTypesAsync())
                .ReturnsAsync(weaponTypeResponses);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.GetAllWeaponTypesAsync();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnStatusCode204_WhenNoWeaponTypesExist()
        {
            // Arrange
            var weaponTypeResponses = new List<WeaponTypeResponse>();

            _weaponTypeServiceMock
                .Setup(x => x.GetAllWeaponTypesAsync())
                .ReturnsAsync(weaponTypeResponses);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.GetAllWeaponTypesAsync();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _weaponTypeServiceMock
                .Setup(x => x.GetAllWeaponTypesAsync())
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.GetAllWeaponTypesAsync();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task FindWeaponTypeByIdAsync_ShouldReturnStatusCode200_WhenWeaponTypeExists()
        {
            // Arrange
            int weaponTypeId = 1;
            var weaponTypeResponse = new WeaponTypeResponse
            {
                WeaponTypeId = weaponTypeId,
                Name = "Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeServiceMock
                .Setup(x => x.FindWeaponTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weaponTypeResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.FindWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task FindWeaponTypeByIdAsync_ShouldReturnStatusCode404_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            int weaponTypeId = 1;

            _weaponTypeServiceMock
                .Setup(x => x.FindWeaponTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.FindWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task FindWeaponTypeByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int weaponTypeId = 1;

            _weaponTypeServiceMock
                .Setup(x => x.FindWeaponTypeByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.FindWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task CreateWeaponTypeAsync_ShouldReturnStatusCode200_WhenWeaponTypeIsSuccessfullyCreated()
        {
            // Arrange
            var weaponTypeRequest = new WeaponTypeRequest
            {
                Name = "Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            var weaponTypeResponse = new WeaponTypeResponse
            {
                WeaponTypeId = 1,
                Name = "Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeServiceMock
                .Setup(x => x.CreateWeaponTypeAsync(It.IsAny<WeaponTypeRequest>()))
                .ReturnsAsync(weaponTypeResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.CreateWeaponTypeAsync(weaponTypeRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateWeaponTypeAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var weaponTypeRequest = new WeaponTypeRequest
            {
                Name = "Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeServiceMock
                .Setup(x => x.CreateWeaponTypeAsync(It.IsAny<WeaponTypeRequest>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.CreateWeaponTypeAsync(weaponTypeRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async Task UpdateWeaponTypeByIdAsync_ShouldReturnStatusCode200_WhenWeaponTypeIsUpdated()
        {
            // Arrange
            int weaponTypeId = 1;
            var weaponTypeRequest = new WeaponTypeRequest
            {
                Name = "Updated Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            var weaponTypeResponse = new WeaponTypeResponse
            {
                WeaponTypeId = weaponTypeId,
                Name = "Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeServiceMock
                .Setup(x => x.UpdateWeaponTypeAsync(It.IsAny<int>(), It.IsAny<WeaponTypeRequest>()))
                .ReturnsAsync(weaponTypeResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.UpdateWeaponTypeByIdAsync(weaponTypeId, weaponTypeRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateWeaponTypeByIdAsync_ShouldReturnStatusCode404_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            int weaponTypeId = 1;
            var weaponTypeRequest = new WeaponTypeRequest
            {
                Name = "Updated Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeServiceMock
                .Setup(x => x.UpdateWeaponTypeAsync(It.IsAny<int>(), It.IsAny<WeaponTypeRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.UpdateWeaponTypeByIdAsync(weaponTypeId, weaponTypeRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWeaponTypeByIdAsync_ShouldReturnStatusCode200_WhenWeaponTypeIsDeleted()
        {
            // Arrange
            int weaponTypeId = 1;
            var weaponTypeResponse = new WeaponTypeResponse
            {
                WeaponTypeId = weaponTypeId,
                Name = "Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeServiceMock
                .Setup(x => x.DeleteWeaponTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(weaponTypeResponse);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.DeleteWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWeaponTypeByIdAsync_ShouldReturnStatusCode404_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            int weaponTypeId = 1;

            _weaponTypeServiceMock
                .Setup(x => x.DeleteWeaponTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.DeleteWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteWeaponTypeByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int weaponTypeId = 1;

            _weaponTypeServiceMock
                .Setup(x => x.DeleteWeaponTypeAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _weaponTypeController.DeleteWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
