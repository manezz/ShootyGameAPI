using ShootyGameAPI.Database.Entities.Interfaces;

namespace ShootyGameAPI.Database.Entities
{
    public class Friend : ISoftDelete
    {
        // Properties
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User Requester { get; set; }
        public User Receiver { get; set; }
    }
}
