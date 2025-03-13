using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class UserWeaponRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly UserWeaponRepository _userWeaponRepository;

        public UserWeaponRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "UserWeaponRepositoryTests")
                .Options;

            _context = new(_options);
            _userWeaponRepository = new(_context);
        }

        [Fact]
        public async Task FindAllUserWeaponsByUserIdAsync_ShouldReturnListOfUserWeapons_WhenUserWeaponsExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                PasswordHash = "Passw0rd",
                PlayerTag = "TestUser#1",
                Email = "admin@mail.com"
            });
            _context.Users.Add(new User
            {
                UserId = 2,
                UserName = "TestUser",
                PasswordHash = "Passw0rd",
                PlayerTag = "TestUser#2",
                Email = "user@mailcom"
            });

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
            _context.Weapons.Add(new Weapon
            {
                WeaponId = 3,
                WeaponTypeId = 1,
                Name = "M4A1",
                Price = 3100,
                ReloadSpeed = 2.7f,
                MagSize = 30,
                FireRate = 600
            });

            _context.UserWeapons.Add(new UserWeapon
            {
                UserId = 1,
                WeaponId = 1
            });
            _context.UserWeapons.Add(new UserWeapon
            {
                UserId = 1,
                WeaponId = 2
            });
            _context.UserWeapons.Add(new UserWeapon
            {
                UserId = 2,
                WeaponId = 3
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _userWeaponRepository.FindAllUserWeaponsByUserIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserWeapon>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindAllUserWeaponsByUserIdAsync_ShouldReturnEmptyList_WhenUserHasNoWeapons()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userWeaponRepository.FindAllUserWeaponsByUserIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<UserWeapon>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindUserWeaponByIdAsync_ShouldReturnUserWeapon_WhenUserWeaponExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.UserWeapons.Add(new UserWeapon
            {
                UserId = 1,
                WeaponId = 1
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _userWeaponRepository.FindUserWeaponByIdAsync(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserWeapon>(result);
            Assert.Equal(1, result?.UserId);
            Assert.Equal(1, result?.WeaponId);
        }

        [Fact]
        public async Task FindUserWeaponByIdAsync_ShouldReturnNull_WhenUserWeaponDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userWeaponRepository.FindUserWeaponByIdAsync(1, 1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateUserWeaponAsync_ShouldAddNewUserWeapon_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            UserWeapon newUserWeapon = new()
            {
                UserId = 1,
                WeaponId = 1
            };

            // Act
            var result = await _userWeaponRepository.CreateUserWeaponAsync(newUserWeapon);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserWeapon>(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal(1, result.WeaponId);
        }

        [Fact]
        public async Task CreateUserWeaponAsync_ShouldFailToAddDuplicateUserWeapon_WhenUserWeaponAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            UserWeapon userWeapon = new()
            {
                UserId = 1,
                WeaponId = 1
            };

            await _userWeaponRepository.CreateUserWeaponAsync(userWeapon);

            // Act
            async Task action() => await _userWeaponRepository.CreateUserWeaponAsync(userWeapon);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task DeleteUserWeaponByIdAsync_ShouldReturnDeletedUserWeapon_WhenUserWeaponIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var userWeapon = new UserWeapon
            {
                UserId = 1,
                WeaponId = 1
            };

            _context.UserWeapons.Add(userWeapon);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userWeaponRepository.DeleteUserWeaponByIdAsync(1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserWeapon>(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal(1, result.WeaponId);
        }

        [Fact]
        public async Task DeleteUserWeaponByIdAsync_ShouldReturnNull_WhenUserWeaponDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _userWeaponRepository.DeleteUserWeaponByIdAsync(1, 1);

            // Assert
            Assert.Null(result);
        }
    }
}
