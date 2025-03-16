using ShootyGameAPI.Database.Entities.Interfaces;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.Database.Entities
{
    public class User : ISoftDelete
    {
        // Properties
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PlayerTag { get; set; }
        public int Money { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public List<UserWeapon> UserWeapons { get; set; }
        public List<Score> Scores { get; set; }
        public List<Friend> FriendsAsRequester { get; set; }
        public List<Friend> FriendsAsReceiver { get; set; }
        public List<FriendReq> SentFriendReqs { get; set; }
        public List<FriendReq> ReceivedFriendReqs { get; set; }
    }
}
