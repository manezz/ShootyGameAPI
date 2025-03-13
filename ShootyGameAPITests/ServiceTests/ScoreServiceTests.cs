using Moq;
using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Repositorys;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ServiceTests
{
    public class ScoreServiceTests
    {
        private readonly ScoreService _scoreService;
        private readonly Mock<IScoreRepository> _scoreRepositoryMock = new();
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        public ScoreServiceTests()
        {
            _scoreService = new ScoreService(
                _scoreRepositoryMock.Object,
                _userRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateScoreAsync_ShouldReturnScoreResponse_WhenCreateIsSuccess()
        {
            // Arrange
            var scoreRequest = new ScoreRequest
            {
                ScoreValue = 100,
                UserId = 1
            };

            var score = new Score
            {
                ScoreId = 1,
                ScoreValue = 100,
                UserId = 1
            };

            _scoreRepositoryMock
                .Setup(x => x.CreateScoreAsync(It.IsAny<Score>()))
                .ReturnsAsync(score);

            // Act
            var result = await _scoreService.CreateScoreAsync(scoreRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScoreResponse>(result);
            Assert.Equal(score.ScoreId, result?.ScoreId);
            Assert.Equal(score.ScoreValue, result?.ScoreValue);
        }

        [Fact]
        public async Task GetAllScoresAsync_ShouldReturnListOfScoreResponses_WhenScoresExist()
        {
            // Arrange
            var scores = new List<Score>
            {
                new Score
                {
                    ScoreId = 1,
                    ScoreValue = 100,
                    UserId = 1
                },
                new Score
                {
                    ScoreId = 2,
                    ScoreValue = 200,
                    UserId = 2
                }
            };

            _scoreRepositoryMock
                .Setup(x => x.GetAllScoresAsync())
                .ReturnsAsync(scores);

            // Act
            var result = await _scoreService.GetAllScoresAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ScoreResponse>>(result);
            Assert.Equal(2, result?.Count);
        }

        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnScoreResponse_WhenScoreExists()
        {
            // Arrange
            var scoreId = 1;

            var score = new Score
            {
                ScoreId = scoreId,
                ScoreValue = 100,
                UserId = 1
            };

            _scoreRepositoryMock
                .Setup(x => x.FindScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(score);

            // Act
            var result = await _scoreService.FindScoreByIdAsync(scoreId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScoreResponse>(result);
            Assert.Equal(score.ScoreId, result?.ScoreId);
            Assert.Equal(score.ScoreValue, result?.ScoreValue);
        }

        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnNull_WhenScoreDoesNotExist()
        {
            // Arrange
            var scoreId = 99;

            _scoreRepositoryMock
                .Setup(x => x.FindScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _scoreService.FindScoreByIdAsync(scoreId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateScoreByIdAsync_ShouldReturnScoreResponse_WhenUpdateIsSuccess()
        {
            // Arrange
            var scoreId = 1;
            var updatedScoreRequest = new ScoreRequest
            {
                ScoreValue = 150,
                UserId = 1
            };

            var updatedScore = new Score
            {
                ScoreId = scoreId,
                ScoreValue = 150,
                UserId = 1
            };

            _scoreRepositoryMock
                .Setup(x => x.UpdateScoreByIdAsync(It.IsAny<int>(), It.IsAny<Score>()))
                .ReturnsAsync(updatedScore);

            // Act
            var result = await _scoreService.UpdateScoreByIdAsync(scoreId, updatedScoreRequest);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScoreResponse>(result);
            Assert.Equal(updatedScore.ScoreId, result?.ScoreId);
            Assert.Equal(updatedScore.ScoreValue, result?.ScoreValue);
        }

        [Fact]
        public async Task UpdateScoreByIdAsync_ShouldReturnNull_WhenScoreDoesNotExist()
        {
            // Arrange
            var scoreId = 99;
            var updatedScoreRequest = new ScoreRequest
            {
                ScoreValue = 150,
                UserId = 1
            };

            _scoreRepositoryMock
                .Setup(x => x.UpdateScoreByIdAsync(It.IsAny<int>(), It.IsAny<Score>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _scoreService.UpdateScoreByIdAsync(scoreId, updatedScoreRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnScoreResponse_WhenDeleteIsSuccess()
        {
            // Arrange
            var scoreId = 1;

            var score = new Score
            {
                ScoreId = scoreId,
                ScoreValue = 100,
                UserId = 1
            };

            _scoreRepositoryMock
                .Setup(x => x.DeleteScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(score);

            // Act
            var result = await _scoreService.DeleteScoreByIdAsync(scoreId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ScoreResponse>(result);
            Assert.Equal(score.ScoreId, result?.ScoreId);
            Assert.Equal(score.ScoreValue, result?.ScoreValue);
        }

        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnNull_WhenScoreDoesNotExist()
        {
            // Arrange
            var scoreId = 99;

            _scoreRepositoryMock
                .Setup(x => x.DeleteScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _scoreService.DeleteScoreByIdAsync(scoreId);

            // Assert
            Assert.Null(result);
        }
    }
}
