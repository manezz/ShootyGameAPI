using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using ShootyGameAPI.Controllers;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Services;

namespace ShootyGameAPITests.ConrollerTests
{
    public class ScoreControllerTests
    {
        private readonly ScoreController _scoreController;
        private readonly Mock<IScoreService> _scoreServiceMock = new();
        private readonly HttpContext httpContext = new DefaultHttpContext();

        public ScoreControllerTests()
        {
            _scoreController = new ScoreController(_scoreServiceMock.Object)
            {
                ControllerContext = new()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task GetAllScoresAsync_ShouldReturnStatusCode200_WhenScoresExist()
        {
            // Arrange
            var scoreResponses = new List<ScoreResponse>
            {
                new ScoreResponse
                {
                    ScoreId = 1,
                    ScoreValue = 100,
                    UserId = 1
                },
                new ScoreResponse
                {
                    ScoreId = 2,
                    ScoreValue = 150,
                    UserId = 2
                }
            };

            _scoreServiceMock
                .Setup(x => x.GetAllScoresAsync())
                .ReturnsAsync(scoreResponses);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.GetAllScoresAsync();

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllScoresAsync_ShouldReturnStatusCode204_WhenNoScoresExist()
        {
            // Arrange
            var scoreResponses = new List<ScoreResponse>();

            _scoreServiceMock
                .Setup(x => x.GetAllScoresAsync())
                .ReturnsAsync(scoreResponses);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.GetAllScoresAsync();

            // Assert
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task GetAllScoresAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            _scoreServiceMock
                .Setup(x => x.GetAllScoresAsync())
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.GetAllScoresAsync();

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        // Test FindScoreByIdAsync
        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnStatusCode200_WhenScoreExists()
        {
            // Arrange
            int scoreId = 1;
            var scoreResponse = new ScoreResponse
            {
                ScoreId = scoreId,
                ScoreValue = 100,
                UserId = 1
            };

            _scoreServiceMock
                .Setup(x => x.FindScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(scoreResponse);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.FindScoreByIdAsync(scoreId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnStatusCode404_WhenScoreDoesNotExist()
        {
            // Arrange
            int scoreId = 1;

            _scoreServiceMock
                .Setup(x => x.FindScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.FindScoreByIdAsync(scoreId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task FindScoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int scoreId = 1;

            _scoreServiceMock
                .Setup(x => x.FindScoreByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.FindScoreByIdAsync(scoreId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        // Test CreateScoreAsync
        [Fact]
        public async Task CreateScoreAsync_ShouldReturnStatusCode200_WhenScoreIsSuccessfullyCreated()
        {
            // Arrange
            var scoreRequest = new ScoreRequest
            {
                ScoreValue = 100,
                UserId = 1
            };
            var scoreResponse = new ScoreResponse
            {
                ScoreId = 1,
                ScoreValue = 100,
                UserId = 1
            };

            _scoreServiceMock
                .Setup(x => x.CreateScoreAsync(It.IsAny<ScoreRequest>()))
                .ReturnsAsync(scoreResponse);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.CreateScoreAsync(scoreRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CreateScoreAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            var scoreRequest = new ScoreRequest
            {
                ScoreValue = 100,
                UserId = 1
            };

            _scoreServiceMock
                .Setup(x => x.CreateScoreAsync(It.IsAny<ScoreRequest>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.CreateScoreAsync(scoreRequest);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }

        // Test UpdateScoreByIdAsync
        [Fact]
        public async Task UpdateScoreByIdAsync_ShouldReturnStatusCode200_WhenScoreIsUpdated()
        {
            // Arrange
            int scoreId = 1;
            var scoreRequest = new ScoreRequest
            {
                ScoreValue = 150,
                UserId = 1
            };
            var scoreResponse = new ScoreResponse
            {
                ScoreId = scoreId,
                ScoreValue = 150,
                UserId = 1
            };

            _scoreServiceMock
                .Setup(x => x.UpdateScoreByIdAsync(It.IsAny<int>(), It.IsAny<ScoreRequest>()))
                .ReturnsAsync(scoreResponse);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.UpdateScoreByIdAsync(scoreId, scoreRequest);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdateScoreByIdAsync_ShouldReturnStatusCode404_WhenScoreDoesNotExist()
        {
            // Arrange
            int scoreId = 1;
            var scoreRequest = new ScoreRequest
            {
                ScoreValue = 150,
                UserId = 1
            };

            _scoreServiceMock
                .Setup(x => x.UpdateScoreByIdAsync(It.IsAny<int>(), It.IsAny<ScoreRequest>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.UpdateScoreByIdAsync(scoreId, scoreRequest);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        // Test DeleteScoreByIdAsync
        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnStatusCode200_WhenScoreIsDeleted()
        {
            // Arrange
            int scoreId = 1;
            var scoreResponse = new ScoreResponse
            {
                ScoreId = scoreId,
                ScoreValue = 100,
                UserId = 1
            };

            _scoreServiceMock
                .Setup(x => x.DeleteScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(scoreResponse);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.DeleteScoreByIdAsync(scoreId);

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnStatusCode404_WhenScoreDoesNotExist()
        {
            // Arrange
            int scoreId = 1;

            _scoreServiceMock
                .Setup(x => x.DeleteScoreByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.DeleteScoreByIdAsync(scoreId);

            // Assert
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task DeleteScoreByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // Arrange
            int scoreId = 1;

            _scoreServiceMock
                .Setup(x => x.DeleteScoreByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("This is an exception"));

            // Act
            var result = (IStatusCodeActionResult)await _scoreController.DeleteScoreByIdAsync(scoreId);

            // Assert
            Assert.Equal(500, result.StatusCode);
        }
    }
}
