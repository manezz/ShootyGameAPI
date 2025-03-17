using Microsoft.EntityFrameworkCore;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPITests.RepositoryTests
{
    public class ScoreRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly ScoreRepository _scoreRepository;

        public ScoreRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ScoreRepositoryTests")
                .Options;

            _context = new(_options);
            _scoreRepository = new(_context);
        }

        [Fact]
        public async Task GetAllScoresAsync_ShouldReturnListOfScores_WhenScoresExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Users.Add(new User
            {
                UserId = 1,
                UserName = "TestUser",
                PlayerTag = "TestUser#1",
                Email = "admin@mail.com",
                PasswordHash = "Passw0rd"
            });
            _context.Users.Add(new User
            {
                UserId = 2,
                UserName = "TestUser",
                PlayerTag = "TestUser#2",
                Email = "admin@mail.com",
                PasswordHash = "Passw0rd"
            });

            _context.Scores.Add(new Score
            {
                ScoreId = 1,
                ScoreValue = 100,
                UserId = 1
            });
            _context.Scores.Add(new Score
            {
                ScoreId = 2,
                ScoreValue = 200,
                UserId = 2
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _scoreRepository.GetAllScoresAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Score>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllScoresAsync_ShouldReturnEmptyList_WhenNoScoresExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _scoreRepository.GetAllScoresAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Score>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnScore_WhenScoreExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Scores.Add(new Score
            {
                ScoreId = 1,
                ScoreValue = 100,
                User = new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _scoreRepository.FindScoreByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Score>(result);
            Assert.Equal(1, result?.ScoreId);
        }

        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnNull_WhenScoreDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _scoreRepository.FindScoreByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateScoreAsync_ShouldAddNewId_WhenSavingToDatabase()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedNewId = 1;

            Score newScore = new()
            {
                ScoreValue = 150,
                User = new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                }
            };

            // Act
            var result = await _scoreRepository.CreateScoreAsync(newScore);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Score>(result);
            Assert.Equal(expectedNewId, result?.ScoreId);
        }

        [Fact]
        public async Task CreateScoreAsync_ShouldFailToAddNewScore_WhenScoreIdAlreadyExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            Score score = new()
            {
                ScoreId = 1,
                ScoreValue = 100,
                UserId = 1
            };

            await _scoreRepository.CreateScoreAsync(score);

            // Act
            async Task action() => await _scoreRepository.CreateScoreAsync(score);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async Task UpdateScoreByIdAsync_ShouldChangeScore_WhenScoreExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var score = new Score
            {
                ScoreId = 1,
                ScoreValue = 100,
                User = new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                }
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            Score updatedScore = new()
            {
                ScoreId = 1,
                ScoreValue = 200,
                UserId = 1
            };

            // Act
            var result = await _scoreRepository.UpdateScoreByIdAsync(1, updatedScore);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Score>(result);
            Assert.Equal(updatedScore.ScoreValue, result?.ScoreValue);
        }

        [Fact]
        public async Task UpdateScoreByIdAsync_ShouldReturnNull_WhenScoreDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var updatedScore = new Score
            {
                ScoreId = 1,
                ScoreValue = 200,
                UserId = 1
            };

            // Act
            var result = await _scoreRepository.UpdateScoreByIdAsync(1, updatedScore);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnDeletedScore_WhenScoreIsDeleted()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            var score = new Score
            {
                ScoreId = 1,
                ScoreValue = 100,
                User = new User
                {
                    UserId = 1,
                    UserName = "TestUser",
                    PlayerTag = "TestUser#1",
                    Email = "admin@mail.com",
                    PasswordHash = "Passw0rd"
                }
            };

            _context.Scores.Add(score);
            await _context.SaveChangesAsync();

            // Act
            var result = await _scoreRepository.DeleteScoreByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Score>(result);
            Assert.Equal(1, result?.ScoreId);
        }

        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnNull_WhenScoreDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            // Act
            var result = await _scoreRepository.DeleteScoreByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
