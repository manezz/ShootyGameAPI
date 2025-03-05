using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class WeaponTypeRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly WeaponTypeRepository _weaponTypeRepository;

        public WeaponTypeRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "WeaponTypeRepositoryTests")
                .Options;

            _context = new(_options);
            _weaponTypeRepository = new(_context);
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnListOfWeaponTypes_WhenWeaponTypesExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.WeaponTypes.Add(new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle",
                EquipmentSlot = 1
            });
            _context.WeaponTypes.Add(new WeaponType
            {
                WeaponTypeId = 2,
                Name = "Pistol",
                EquipmentSlot = 2
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _weaponTypeRepository.GetAllWeaponTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<WeaponType>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllWeaponTypesAsync_ShouldReturnEmptyList_WhenNoWeaponTypesExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _weaponTypeRepository.GetAllWeaponTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<WeaponType>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindWeaponTypeByIdAsync_ShouldReturnWeaponType_WhenWeaponTypeExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.WeaponTypes.Add(new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle",
                EquipmentSlot = 1
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _weaponTypeRepository.FindWeaponTypeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponType>(result);
            Assert.Equal(1, result?.WeaponTypeId);
        }

        [Fact]
        public async Task FindWeaponTypeByIdAsync_ShouldReturnNull_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _weaponTypeRepository.FindWeaponTypeByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateWeaponTypeAsync_ShouldAddNewId_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;

            WeaponType newWeaponType = new()
            {
                Name = "Shotgun",
                EquipmentSlot = 1
            };

            // Act
            var result = await _weaponTypeRepository.CreateWeaponTypeAsync(newWeaponType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponType>(result);
            Assert.Equal(expectedNewId, result?.WeaponTypeId);
        }

        [Fact]
        public async Task CreateWeaponTypeAsync_ShouldFailToAddNewWeaponType_WhenWeaponTypeIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            WeaponType existingWeaponType = new()
            {
                WeaponTypeId = 1,
                Name = "Rifle",
                EquipmentSlot = 1
            };

            await _weaponTypeRepository.CreateWeaponTypeAsync(existingWeaponType);

            // Act
            async Task action() => await _weaponTypeRepository.CreateWeaponTypeAsync(existingWeaponType);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task UpdateWeaponTypeByIdAsync_ShouldChangeWeaponType_WhenWeaponTypeExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var weaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle",
                EquipmentSlot = 1
            };

            _context.WeaponTypes.Add(weaponType);
            await _context.SaveChangesAsync();

            WeaponType updatedWeaponType = new()
            {
                WeaponTypeId = 1,
                Name = "Assault Rifle",
                EquipmentSlot = 1
            };

            // Act
            var result = await _weaponTypeRepository.UpdateWeaponTypeByIdAsync(1, updatedWeaponType);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponType>(result);
            Assert.Equal("Assault Rifle", result?.Name);
            Assert.Equal(updatedWeaponType.EquipmentSlot, result?.EquipmentSlot);
        }

        [Fact]
        public async Task UpdateWeaponTypeByIdAsync_ShouldReturnNull_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var updatedWeaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Assault Rifle",
                EquipmentSlot = 1
            };

            // Act
            var result = await _weaponTypeRepository.UpdateWeaponTypeByIdAsync(1, updatedWeaponType);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteWeaponTypeByIdAsync_ShouldReturnDeletedWeaponType_WhenWeaponTypeIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var weaponType = new WeaponType
            {
                WeaponTypeId = 1,
                Name = "Rifle",
                EquipmentSlot = 1
            };

            _context.WeaponTypes.Add(weaponType);
            await _context.SaveChangesAsync();

            // Act
            var result = await _weaponTypeRepository.DeleteWeaponTypeByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<WeaponType>(result);
            Assert.Equal(1, result.WeaponTypeId);
        }

        [Fact]
        public async Task DeleteWeaponTypeByIdAsync_ShouldReturnNull_WhenWeaponTypeDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _weaponTypeRepository.DeleteWeaponTypeByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
