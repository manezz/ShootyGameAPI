using ShootyGameAPI.Database.Entities.Interfaces;

namespace ShootyGameAPI.Database.Entities
{
    public class Score : ISoftDelete
    {
        // Properties
        public int ScoreId { get; set; }
        public int UserId { get; set; }
        public int ScoreValue { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User User { get; set; }
    }
}
