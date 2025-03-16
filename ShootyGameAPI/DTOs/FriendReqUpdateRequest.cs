using ShootyGameAPI.Helpers;
using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class FriendReqUpdateRequest
    {
        [Required]
        public FriendReqStatus Status { get; set; }
    }
}
