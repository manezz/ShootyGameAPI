using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class WeaponRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly WeaponRepository _weaponRepository;

        public WeaponRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "WeaponRepositoryTests")
                .Options;

            _context = new(_options);
            _weaponRepository = new(_context);
        }

        [Fact]
        public async Task GetAllWeaponsAsync_ShouldReturnListOfWeapons_WhenWeaponsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.WeaponTypes.Add(new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle"
            });
            _context.WeaponTypes.Add(new WeaponType
            {
                WeaponTypeId = 2,
                Name = "Pistol"
            });
            _context.Weapons.Add(new Weapon
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47",
                Price = 2500,
                ReloadSpeed = 2.5f,
                MagSize = 30,
                FireRate = 600
            });
            _context.Weapons.Add(new Weapon
            {
                WeaponId = 2,
                WeaponTypeId = 2,
                Name = "Desert Eagle",
                Price = 1500,
                ReloadSpeed = 2.0f,
                MagSize = 7,
                FireRate = 300
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _weaponRepository.GetAllWeaponsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Weapon>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllWeaponsAsync_ShouldReturnEmptyList_WhenNoWeaponsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _weaponRepository.GetAllWeaponsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Weapon>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnWeapon_WhenWeaponExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var weaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle"
            };

            _context.WeaponTypes.Add(weaponType);

            _context.Weapons.Add(new Weapon
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47",
                Price = 2500,
                ReloadSpeed = 2.5f,
                MagSize = 30,
                FireRate = 600
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _weaponRepository.FindWeaponByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Weapon>(result);
            Assert.Equal(1, result?.WeaponId);
        }

        [Fact]
        public async Task FindWeaponByIdAsync_ShouldReturnNull_WhenWeaponDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _weaponRepository.FindWeaponByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateWeaponAsync_ShouldAddNewId_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var weaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle"
            };

            _context.WeaponTypes.Add(weaponType);

            int expectedNewId = 1;

            Weapon newWeapon = new()
            {
                WeaponTypeId = 1,
                Name = "AK-47",
                Price = 2000,
                ReloadSpeed = 2.5f,
                MagSize = 30,
                FireRate = 600
            };

            // Act
            var result = await _weaponRepository.CreateWeaponAsync(newWeapon);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Weapon>(result);
            Assert.Equal(expectedNewId, result?.WeaponId);
        }

        [Fact]
        public async Task CreateWeaponAsync_ShouldFailToAddNewWeapon_WhenWeaponIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Weapon weapon = new()
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47",
                Price = 2500,
                ReloadSpeed = 2.5f,
                MagSize = 30,
                FireRate = 600
            };

            await _weaponRepository.CreateWeaponAsync(weapon);

            // Act
            async Task action() => await _weaponRepository.CreateWeaponAsync(weapon);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task UpdateWeaponByIdAsync_ShouldChangeValuesOnWeapon_WhenWeaponExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var weaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle"
            };

            _context.WeaponTypes.Add(weaponType);

            var weapon = new Weapon
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47",
                Price = 2500,
                ReloadSpeed = 2.5f,
                MagSize = 30,
                FireRate = 600
            };

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            Weapon updatedWeapon = new()
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47 Custom",
                Price = 2700,
                ReloadSpeed = 2.0f,
                MagSize = 35,
                FireRate = 650
            };

            // Act
            var result = await _weaponRepository.UpdateWeaponByIdAsync(1, updatedWeapon);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Weapon>(result);
            Assert.Equal("AK-47 Custom", result?.Name);
            Assert.Equal(updatedWeapon.Price, result?.Price);
            Assert.Equal(updatedWeapon.ReloadSpeed, result?.ReloadSpeed);
            Assert.Equal(updatedWeapon.MagSize, result?.MagSize);
            Assert.Equal(updatedWeapon.FireRate, result?.FireRate);
        }

        [Fact]
        public async Task UpdateWeaponByIdAsync_ShouldReturnNull_WhenWeaponDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var updatedWeapon = new Weapon
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47 Custom",
                Price = 2700,
                ReloadSpeed = 2.0f,
                MagSize = 35,
                FireRate = 650
            };

            // Act
            var result = await _weaponRepository.UpdateWeaponByIdAsync(1, updatedWeapon);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnDeletedWeapon_WhenWeaponIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var weaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle"
            };

            _context.WeaponTypes.Add(weaponType);

            var weapon = new Weapon
            {
                WeaponId = 1,
                WeaponTypeId = 1,
                Name = "AK-47",
                Price = 2500,
                ReloadSpeed = 2.5f,
                MagSize = 30,
                FireRate = 600
            };

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            // Act
            var result = await _weaponRepository.DeleteWeaponByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Weapon>(result);
            Assert.Equal(1, result.WeaponId);
        }

        [Fact]
        public async Task DeleteWeaponByIdAsync_ShouldReturnNull_WhenWeaponDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _weaponRepository.DeleteWeaponByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
