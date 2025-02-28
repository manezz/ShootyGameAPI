using ShootyGameAPI.Database.Entities.Interfaces;

namespace ShootyGameAPI.Database.Entities
{
    public class Friend : ISoftDelete
    {
        // Properties
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User User1 { get; set; }
        public User User2 { get; set; }
    }
}
