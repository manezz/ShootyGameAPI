using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.Database;
using Microsoft.EntityFrameworkCore;

namespace ShootyGameAPI.Repositorys
{
    public interface IScoreRepository
    {
        Task<List<Score>> GetAllScoresAsync();
        Task<Score?> FindScoreByIdAsync(int id);
        Task<Score> CreateScoreAsync(Score newScore);
        Task<Score?> UpdateScoreByIdAsync(int scoreId, Score updatedScore);
        Task<Score?> DeleteScoreByIdAsync(int id);
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
            return await _context.Scores.ToListAsync();
        }

        public async Task<Score?> FindScoreByIdAsync(int id)
        {
            return await _context.Scores.FindAsync(id);
        }

        public async Task<Score> CreateScoreAsync(Score newScore)
        {
            _context.Scores.Add(newScore);
            await _context.SaveChangesAsync();
            return newScore;
        }

        public async Task<Score?> UpdateScoreByIdAsync(int scoreId, Score updatedScore)
        {
            var score = await FindScoreByIdAsync(updatedScore.ScoreId);
            if (score != null)
            {
                score.ScoreValue = updatedScore.ScoreValue;

                await _context.SaveChangesAsync();
                return updatedScore;
            }
            return score;
        }

        public async Task<Score?> DeleteScoreByIdAsync(int id)
        {
            var score = await FindScoreByIdAsync(id);
            if (score != null)
            {
                _context.Scores.Remove(score);
                await _context.SaveChangesAsync();
            }
            return score;
        }
    }
}
