using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Database.Entities
{
    public class FriendRequest
    {
        // Properties
        public int FriendRequestId { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ResponseAt { get; set; }
        public FriendRequestStatus Status { get; set; }

        // Navigation properties
        public User Requester { get; set; }
        public User Receiver { get; set; }
    }
}
