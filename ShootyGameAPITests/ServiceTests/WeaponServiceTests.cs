using Moq;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Repositorys;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ServiceTests
{
    public class WeaponServiceTests
    {
        private readonly WeaponService _weaponService;
        private readonly Mock<IWeaponRepository> _weaponRepositoryMock = new();
        private readonly Mock<IWeaponTypeRepository> _weaponTypeRepositoryMock = new();

        public WeaponServiceTests()
        {
            _weaponService = new WeaponService(
                _weaponRepositoryMock.Object,
                _weaponTypeRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateWeaponAsync_ShouldReturnWeaponResponse_WhenCreateIsSuccess()
        {
            // Arrange
            var weaponRequest = new WeaponRequest
            {
                Name = "M4",
                Price = 200,
                ReloadSpeed = 1.0f,
                MagSize = 8,
                FireRate = 400,
                FireMode = FireMode.Auto,
                WeaponTypeId = 1
            };

            var weapon = new Weapon
            {
                WeaponId = 1,
                Name = "M1911",
                Price = 200,
                ReloadSpeed = 1.0f,
                MagSize = 8,
                FireRate = 200,
                FireMode = FireMode.Single,
                WeaponType = new WeaponType
                {
                    WeaponTypeId = 1,
                    Name = "Pistol",
                    EquipmentSlot = EquipmentSlot.Secondary
                }
            };

            _weaponRepositoryMock
                .Setup(x => x.CreateWeaponAsync(It.IsAny<Weapon>()))
                .ReturnsAsync(weapon);

            // Act
            var result = await _weaponService.CreateWeaponAsync(weaponRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponResponse>(result);
            Assert.Equal(weapon.WeaponId, result?.WeaponId);
            Assert.Equal(weapon.Name, result?.Name);
        }

        [Fact]
        public async Task GetAllWeaponsAsync_ShouldReturnListOfWeaponResponses_WhenWeaponsExist()
        {
            // Arrange
            var weapons = new List<Weapon>
        {
            new Weapon
            {
                WeaponId = 1,
                Name = "M1911",
                Price = 100,
                ReloadSpeed = 2.0f,
                MagSize = 12,
                FireRate = 200,
                FireMode = FireMode.Single,
                WeaponType = new WeaponType
                {
                    WeaponTypeId = 1,
                    Name = "Pistol",
                    EquipmentSlot = EquipmentSlot.Secondary
                }
            },
            new Weapon
            {
                WeaponId = 2,
                Name = "M4",
                Price = 300,
                ReloadSpeed = 1.5f,
                MagSize = 30,
                FireRate = 3,
                FireMode = FireMode.Auto,
                WeaponType = new WeaponType
                {
                    WeaponTypeId = 2,
                    Name = "Rifle",
                    EquipmentSlot = EquipmentSlot.Primary
                }
            }
        };

            _weaponRepositoryMock
                .Setup(x => x.GetAllWeaponsAsync())
                .ReturnsAsync(weapons);

            // Act
            var result = await _weaponService.GetAllWeaponsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<WeaponResponse>>(result);
            Assert.Equal(2, result?.Count);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnWeaponResponse_WhenWeaponExists()
        {
            // Arrange
            var weaponId = 1;

            var weapon = new Weapon
            {
                WeaponId = weaponId,
                Name = "Pistol",
                Price = 100,
                ReloadSpeed = 2.0f,
                MagSize = 12,
                FireRate = 200,
                FireMode = FireMode.Single,
                WeaponType = new WeaponType
                {
                    WeaponTypeId = 1,
                    Name = "Pistol",
                    EquipmentSlot = EquipmentSlot.Secondary
                }
            };

            _weaponRepositoryMock
                .Setup(x => x.FindWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weapon);

            // Act
            var result = await _weaponService.FindWeaponByIdAsync(weaponId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponResponse>(result);
            Assert.Equal(weapon.WeaponId, result?.WeaponId);
            Assert.Equal(weapon.Name, result?.Name);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnNull_WhenWeaponDoesNotExist()
        {
            // Arrange
            var weaponId = 1;

            _weaponRepositoryMock
                .Setup(x => x.FindWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _weaponService.FindWeaponByIdAsync(weaponId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateWeaponByIdAsync_ShouldReturnWeaponResponse_WhenUpdateIsSuccess()
        {
            // Arrange
            var weaponId = 1;
            var updatedWeaponRequest = new WeaponRequest
            {
                Name = "Updated M1911",
                Price = 150,
                ReloadSpeed = 1.5f,
                MagSize = 15,
                FireRate = 200,
                FireMode = FireMode.Auto,
                WeaponTypeId = 1,
            };

            var updatedWeapon = new Weapon
            {
                WeaponId = weaponId,
                Name = "M1911",
                Price = 1500,
                ReloadSpeed = 1.7f,
                MagSize = 12,
                FireRate = 400,
                FireMode = FireMode.Single,
                WeaponType = new WeaponType
                {
                    WeaponTypeId = 1,
                    Name = "Pistol",
                    EquipmentSlot = EquipmentSlot.Secondary
                }
            };

            _weaponRepositoryMock
                .Setup(x => x.UpdateWeaponByIdAsync(It.IsAny<int>(), It.IsAny<Weapon>()))
                .ReturnsAsync(updatedWeapon);

            // Act
            var result = await _weaponService.UpdateWeaponByIdAsync(weaponId, updatedWeaponRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponResponse>(result);
            Assert.Equal(updatedWeapon.WeaponId, result?.WeaponId);
            Assert.Equal(updatedWeapon.Name, result?.Name);
            Assert.Equal(updatedWeapon.Price, result?.Price);
        }

        [Fact]
        public async Task UpdateWeaponByIdAsync_ShouldReturnNull_WhenWeaponDoesNotExist()
        {
            // Arrange
            var weaponId = 1;
            var updatedWeaponRequest = new WeaponRequest
            {
                Name = "Updated M1911",
                Price = 150,
                ReloadSpeed = 1.5f,
                MagSize = 15,
                FireRate = 6,
                FireMode = FireMode.Auto,
                WeaponTypeId = 1
            };

            _weaponRepositoryMock
                .Setup(x => x.UpdateWeaponByIdAsync(It.IsAny<int>(), It.IsAny<Weapon>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _weaponService.UpdateWeaponByIdAsync(weaponId, updatedWeaponRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnWeaponResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            var weaponId = 1;

            var weapon = new Weapon
            {
                WeaponId = weaponId,
                Name = "M1911",
                Price = 100,
                ReloadSpeed = 2.0f,
                MagSize = 12,
                FireRate = 500,
                FireMode = FireMode.Single,
                WeaponType = new WeaponType
                {
                    WeaponTypeId = 1,
                    Name = "Pistol",
                    EquipmentSlot = EquipmentSlot.Secondary
                }
            };

            _weaponRepositoryMock
                .Setup(x => x.DeleteWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(weapon);

            // Act
            var result = await _weaponService.DeleteWeaponByIdAsync(weaponId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponResponse>(result);
            Assert.Equal(weapon.WeaponId, result?.WeaponId);
            Assert.Equal(weapon.Name, result?.Name);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnNull_WhenWeaponDoesNotExist()
        {
            // Arrange
            var weaponId = 1;

            _weaponRepositoryMock
                .Setup(x => x.DeleteWeaponByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _weaponService.DeleteWeaponByIdAsync(weaponId);

            // Assert
            Assert.Null(result);
        }
    }
}
