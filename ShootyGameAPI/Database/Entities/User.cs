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
        public int Money { get; set; } = 0;
        public Role Role { get; set; } = Role.User;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }

        // Navigation properties
        public List<UserWeapon> UserWeapons { get; set; }
        public List<Score> Scores { get; set; }
        public List<Friend> Friends1 { get; set; }
        public List<Friend> Friends2 { get; set; }
        public List<FriendRequest> FriendRequests1 { get; set; }
        public List<FriendRequest> FriendRequests2 { get; set; }
    }
}
