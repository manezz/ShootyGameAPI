using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IScoreRepository
    {
        Task<List<Score>> GetAllScoresAsync();
        Task<Score?> FindScoreByIdAsync(int scoreId);
        Task<Score?> CreateScoreAsync(Score newScore);
        Task<Score?> UpdateScoreByIdAsync(int scoreId, Score updatedScore);
        Task<Score?> DeleteScoreByIdAsync(int scoreId);
    }

    public class ScoreRepository : IScoreRepository
    {
        private readonly DatabaseContext _context;

        public ScoreRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Score>> GetAllScoresAsync()
        {
            return await _context.Scores
                .OrderByDescending(x => x.ScoreValue)
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task<Score?> FindScoreByIdAsync(int scoreId)
        {
            return await _context.Scores
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.ScoreId == scoreId);
        }

        public async Task<Score?> CreateScoreAsync(Score newScore)
        {
            _context.Scores.Add(newScore);
            await _context.SaveChangesAsync();
            return await FindScoreByIdAsync(newScore.ScoreId);
        }

        public async Task<Score?> UpdateScoreByIdAsync(int scoreId, Score updatedScore)
        {
            var score = await FindScoreByIdAsync(scoreId);
            if (score != null)
            {
                score.ScoreValue = updatedScore.ScoreValue;
                score.AverageAccuracy = updatedScore.AverageAccuracy;
                score.RoundTime = updatedScore.RoundTime;

                await _context.SaveChangesAsync();
                return await FindScoreByIdAsync(scoreId);
            }
            return score;
        }

        public async Task<Score?> DeleteScoreByIdAsync(int scoreId)
        {
            var score = await FindScoreByIdAsync(scoreId);
            if (score != null)
            {
                _context.Scores.Remove(score);
                await _context.SaveChangesAsync();
            }
            return score;
        }
    }
}
