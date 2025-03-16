using ShootyGameAPI.Database.Entities.Interfaces;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Database.Entities
{
    public class FriendReq : ISoftDelete
    {
        // Properties
        public int FriendReqId { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ResponseAt { get; set; }
        public FriendReqStatus Status { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User Requester { get; set; }
        public User Receiver { get; set; }
    }
}
