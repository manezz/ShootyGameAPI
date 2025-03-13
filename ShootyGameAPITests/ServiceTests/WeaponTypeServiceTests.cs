using Moq;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ServiceTests
{
    public class WeaponTypeServiceTests
    {
        private readonly WeaponTypeService _weaponTypeService;
        private readonly Mock<IWeaponTypeRepository> _weaponTypeRepositoryMock = new();

        public WeaponTypeServiceTests()
        {
            _weaponTypeService = new WeaponTypeService(_weaponTypeRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnListOfWeaponTypeResponses_WhenWeaponTypesExist()
        {
            // Arrange
            List<WeaponType> weaponTypes = new()
                {
                    new()
                    {
                        WeaponTypeId = 1,
                        Name = "Asualt Rifle",
                        EquipmentSlot = EquipmentSlot.Primary
                    },
                    new()
                    {
                        WeaponTypeId = 2,
                        Name = "Handgun",
                        EquipmentSlot = EquipmentSlot.Secondary
                    }
            };

            _weaponTypeRepositoryMock
                .Setup(x => x.GetAllWeaponTypesAsync())
                .ReturnsAsync(weaponTypes);

            // Act
            var result = await _weaponTypeService.GetAllWeaponTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<WeaponTypeResponse>>(result);
            Assert.Equal(2, result?.Count);
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnEmptyList_WhenNoWeaponTypesExist()
        {
            // Arrange
            List<WeaponType> weaponTypes = new();

            _weaponTypeRepositoryMock
                .Setup(x => x.GetAllWeaponTypesAsync())
                .ReturnsAsync(weaponTypes);

            // Act
            var result = await _weaponTypeService.GetAllWeaponTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<WeaponTypeResponse>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetWeaponTypeByIdAsync_ShouldReturnWeaponTypeResponse_WhenWeaponTypeExists()
        {
            // Arrange
            int weaponTypeId = 1;

            WeaponType weaponType = new()
            {
                WeaponTypeId = weaponTypeId,
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeRepositoryMock
                .Setup(x => x.FindWeaponTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weaponType);

            // Act
            var result = await _weaponTypeService.FindWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponTypeResponse>(result);
            Assert.Equal(weaponType.WeaponTypeId, result?.WeaponTypeId);
            Assert.Equal(weaponType.Name, result?.Name);
        }

        [Fact]
        public async Task GetWeaponTypeByIdAsync_ShouldReturnNull_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            int weaponTypeId = 1;

            _weaponTypeRepositoryMock
                .Setup(x => x.FindWeaponTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _weaponTypeService.FindWeaponTypeByIdAsync(weaponTypeId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateWeaponTypeAsync_ShouldReturnWeaponTypeResponse_WhenCreationIsSuccessful()
        {
            // Arrange
            WeaponTypeRequest newWeaponType = new()
            {
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };
            int weaponTypeId = 1;

            WeaponType weaponType = new()
            {
                WeaponTypeId = weaponTypeId,
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeRepositoryMock
                .Setup(x => x.CreateWeaponTypeAsync(It.IsAny<WeaponType>()))
                .ReturnsAsync(weaponType);

            // Act
            var result = await _weaponTypeService.CreateWeaponTypeAsync(newWeaponType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponTypeResponse>(result);
            Assert.Equal(weaponType.WeaponTypeId, result?.WeaponTypeId);
            Assert.Equal(weaponType.Name, result?.Name);
        }

        [Fact]
        public async Task CreateWeaponTypeAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            WeaponTypeRequest newWeaponType = new()
            {
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeRepositoryMock
                .Setup(x => x.CreateWeaponTypeAsync(It.IsAny<WeaponType>()))
                .ReturnsAsync(() => null!);

            // Act
            var result = await _weaponTypeService.CreateWeaponTypeAsync(newWeaponType);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateWeaponTypeAsync_ShouldReturnWeaponTypeResponse_WhenUpdateIsSuccessful()
        {
            // Arrange
            WeaponTypeRequest updateWeaponType = new()
            {
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };
            int weaponTypeId = 1;

            WeaponType updatedWeaponTypeEntity = new()
            {
                WeaponTypeId = weaponTypeId,
                Name = updateWeaponType.Name,
                EquipmentSlot = updateWeaponType.EquipmentSlot
            };

            _weaponTypeRepositoryMock
                .Setup(x => x.UpdateWeaponTypeByIdAsync(It.IsAny<int>(), It.IsAny<WeaponType>()))
                .ReturnsAsync(updatedWeaponTypeEntity);

            // Act
            var result = await _weaponTypeService.UpdateWeaponTypeAsync(weaponTypeId, updateWeaponType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponTypeResponse>(result);
            Assert.Equal(weaponTypeId, result?.WeaponTypeId);
            Assert.Equal(updateWeaponType.Name, result?.Name);
            Assert.Equal(updateWeaponType.EquipmentSlot, result?.EquipmentSlot);
        }

        [Fact]
        public async Task UpdateWeaponTypeAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
        {
            // Arrange
            WeaponTypeRequest updateWeaponType = new()
            {
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };
            int weaponTypeId = 1;

            _weaponTypeRepositoryMock
                .Setup(x => x.UpdateWeaponTypeByIdAsync(It.IsAny<int>(), It.IsAny<WeaponType>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _weaponTypeService.UpdateWeaponTypeAsync(weaponTypeId, updateWeaponType);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteWeaponTypeAsync_ShouldReturnWeaponTypeResponse_WhenDeletionIsSuccessful()
        {
            // Arrange
            int weaponTypeId = 1;

            WeaponType weaponType = new()
            {
                WeaponTypeId = weaponTypeId,
                Name = "Asualt Rifle",
                EquipmentSlot = EquipmentSlot.Primary
            };

            _weaponTypeRepositoryMock
                .Setup(x => x.DeleteWeaponTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weaponType);

            // Act
            var result = await _weaponTypeService.DeleteWeaponTypeAsync(weaponTypeId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponTypeResponse>(result);
            Assert.Equal(weaponType.WeaponTypeId, result?.WeaponTypeId);
        }

        [Fact]
        public async Task DeleteWeaponTypeAsync_ShouldReturnNull_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            int weaponTypeId = 1;

            _weaponTypeRepositoryMock
                .Setup(x => x.DeleteWeaponTypeByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _weaponTypeService.DeleteWeaponTypeAsync(weaponTypeId);

            // Assert
            Assert.Null(result);
        }
    }
}
