using Microsoft.AspNetCore.SignalR;
using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.DTOs
{
    public class UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PlayerTag { get; set; }
        public int Money { get; set; }
        public Role Role { get; set; }
        public List<User_WeaponsResponse> Weapons { get; set; } = new();
        public List<User_ScoreResponse> Scores { get; set; } = new();
        public List<User_FriendReqResponse> SentFriendRequests { get; set; } = new();
        public List<User_FriendReqResponse> ReceivedFriendRequests { get; set; } = new();
        public List<User_FriendResponse> Friends { get; set; } = new();
    }

    public class User_WeaponsResponse
    {
        public int WeaponId { get; set; }
        public string Name { get; set; }
        public float ReloadSpeed { get; set; }
        public int MagSize { get; set; }
        public int FireRate { get; set; }
        public FireMode FireMode { get; set; }
        public User_Weapon_WeaponTypeResponse WeaponType { get; set; }
    }

    public class User_Weapon_WeaponTypeResponse
    {
        public int WeaponTypeId { get; set; }
        public string Name { get; set; }
        public EquipmentSlot EquipmentSlot { get; set; }
    }

    public class User_ScoreResponse
    {
        public int ScoreId { get; set; }
        public int ScoreValue { get; set; }
        public float AverageAccuracy { get; set; }
        public float RoundTime { get; set; }
    }

    public class User_FriendReqResponse
    {
        public int FriendRequestId { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public FriendReqStatus Status { get; set; }
    }

    public class User_FriendResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PlayerTag { get; set; }
    }
}
