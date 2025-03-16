namespace ShootyGameAPI.DTOs
{
    public class FriendResponse
    {
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public Friend_UserResponse Requester { get; set; } = new();
        public Friend_UserResponse Receiver { get; set; } = new();
    }

    public class Friend_UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PlayerTag { get; set; }
    }
}
