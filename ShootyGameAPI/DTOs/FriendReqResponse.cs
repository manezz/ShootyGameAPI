using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.DTOs
{
    public class FriendReqResponse
    {
        public int FriendRequestId { get; set; }
        public int RequesterId { get; set; }
        public int ReceiverId { get; set; }
        public FriendReqStatus Status { get; set; }
        public FriendReq_UserResponse Requester { get; set; } = new();
        public FriendReq_UserResponse Receiver { get; set; } = new();
    }

    public class FriendReq_UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PlayerTag { get; set; }
    }
}
