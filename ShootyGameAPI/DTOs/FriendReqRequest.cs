using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class FriendReqRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "RequesterId must be at minimum 1.")]
        public int RequesterId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ReceiverId must be at minimum 1.")]
        public int ReceiverId { get; set; }
    }
}
