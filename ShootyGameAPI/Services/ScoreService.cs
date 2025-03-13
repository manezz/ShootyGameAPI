using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPI.Services
{
    public interface IScoreService
    {
        Task<List<ScoreResponse>> GetAllScoresAsync();
        Task<ScoreResponse?> FindScoreByIdAsync(int scoreId);
        Task<ScoreResponse?> CreateScoreAsync(ScoreRequest newScore);
        Task<ScoreResponse?> UpdateScoreByIdAsync(int scoreId, ScoreRequest updatedScore);
        Task<ScoreResponse?> DeleteScoreByIdAsync(int scoreId);
    }

    public class ScoreService : IScoreService
    {
        private readonly IScoreRepository _scoreRepository;
        private readonly IUserRepository _userRepository;

        public ScoreService(IScoreRepository scoreRepository, IUserRepository userRepository)
        {
            _scoreRepository = scoreRepository;
            _userRepository = userRepository;
        }

        private ScoreResponse MapScoreToScoreResponse(Score score)
        {
            return new ScoreResponse
            {
                ScoreId = score.ScoreId,
                ScoreValue = score.ScoreValue,
                AverageAccuracy = score.AverageAccuracy,
                RoundTime = score.RoundTime,
                UserId = score.UserId
            };
        }

        private Score MapScoreRequestToScore(ScoreRequest scoreRequest)
        {
            return new Score
            {
                ScoreValue = scoreRequest.ScoreValue,
                AverageAccuracy = scoreRequest.AverageAccuracy,
                RoundTime = scoreRequest.RoundTime,
                UserId = scoreRequest.UserId
            };
        }

        public async Task<List<ScoreResponse>> GetAllScoresAsync()
        {
            var scores = await _scoreRepository.GetAllScoresAsync();
            return scores.Select(MapScoreToScoreResponse).ToList();
        }

        public async Task<ScoreResponse?> FindScoreByIdAsync(int scoreId)
        {
            var score = await _scoreRepository.FindScoreByIdAsync(scoreId);

            if (score == null)
            {
                return null;
            }

            return MapScoreToScoreResponse(score);
        }

        private async Task<User?> AddMoneyToUser(int userId, int money)
        {
            var user = _userRepository.FindUserByIdAsync(userId).Result;

            if (user == null)
            {
                return null;
            }

            user.Money += money;

            var updatedUser = await _userRepository.UpdateUserByIdAsync(userId, user);

            if (updatedUser == null)
            {
                return null;
            }

            return updatedUser;
        }

        public async Task<ScoreResponse?> CreateScoreAsync(ScoreRequest newScore)
        {
            var createdScore = await _scoreRepository.CreateScoreAsync(MapScoreRequestToScore(newScore));

            if (createdScore == null)
            {
                return null;
            }

            var user = _userRepository.FindUserByIdAsync(newScore.UserId).Result;

            await AddMoneyToUser(newScore.UserId, newScore.MoneyEarned);

            return MapScoreToScoreResponse(createdScore);
        }

        public async Task<ScoreResponse?> UpdateScoreByIdAsync(int scoreId, ScoreRequest updatedScore)
        {
            var updatedEntity = await _scoreRepository.UpdateScoreByIdAsync(scoreId, MapScoreRequestToScore(updatedScore));

            if (updatedEntity == null)
            {
                return null;
            }

            return MapScoreToScoreResponse(updatedEntity);
        }

        public async Task<ScoreResponse?> DeleteScoreByIdAsync(int scoreId)
        {
            var deletedScore = await _scoreRepository.DeleteScoreByIdAsync(scoreId);

            if (deletedScore == null)
            {
                return null;
            }

            return MapScoreToScoreResponse(deletedScore);
        }
    }
}
