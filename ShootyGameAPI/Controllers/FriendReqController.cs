using Microsoft.AspNetCore.Mvc;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendReqController : Controller
    {
        private readonly IFriendReqService _friendReqService;

        public FriendReqController(IFriendReqService friendReqService)
        {
            _friendReqService = friendReqService;
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet("requester/{requesterId}")]
        public async Task<IActionResult> FindFriendReqByRequesterIdAsync(int requesterId)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != requesterId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var friendRequests = await _friendReqService.FindFriendReqByRequesterIdAsync(requesterId);

                if (friendRequests.Count == 0)
                {
                    return NoContent();
                }

                return Ok(friendRequests);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet("receiver/{receiverId}")]
        public async Task<IActionResult> FindFriendReqByReceiverIdAsync(int receiverId)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != receiverId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var friendRequests = await _friendReqService.FindFriendReqByReceiverIdAsync(receiverId);

                if (friendRequests.Count == 0)
                {
                    return NoContent();
                }

                return Ok(friendRequests);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateFriendReqAsync([FromBody] FriendReqRequest friendRequest)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != friendRequest.RequesterId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var createdRequest = await _friendReqService.CreateFriendReqAsync(friendRequest);

                if (createdRequest == null)
                {
                    return BadRequest("Friend request could not be created.");
                }

                return Ok(createdRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPut("{friendRequestId}")]
        public async Task<IActionResult> UpdateFriendReqByIdAsync(int friendRequestId, [FromBody] FriendReqUpdateRequest friendRequestUpdateRequest)
        {
            try
            {
                var currentFriendReq = await _friendReqService.FindFriendReqByIdAsync(friendRequestId);

                if (currentFriendReq == null)
                {
                    return NotFound(new { message = "Friend request not found." });
                }

                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != currentFriendReq.RequesterId && currentUser.UserId != currentFriendReq.ReceiverId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var updatedRequest = await _friendReqService.UpdateFriendReqByIdAsync(friendRequestId, friendRequestUpdateRequest);

                if (updatedRequest == null)
                {
                    return NotFound();
                }

                return Ok(updatedRequest);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
