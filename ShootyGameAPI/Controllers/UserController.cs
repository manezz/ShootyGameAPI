using Microsoft.AspNetCore.Mvc;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] SignInRequest signInRequest)
        {
            try
            {
                var signInResponse = await _userService.AuthenticateAsync(signInRequest);

                if (signInResponse == null)
                {
                    return NotFound();
                }

                return Ok(signInResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPost]
        [Route("weapons")]
        public async Task<IActionResult> AddWeaponToUserAsync([FromBody] UserWeaponRequest userWeaponRequest)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != userWeaponRequest.UserId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var userResponse = await _userService.AddWeaponToUserAsync(userWeaponRequest);

                if (userResponse == null)
                {
                    return NotFound();
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{userId}/weapons/{weaponId}")]
        public async Task<IActionResult> RemoveWeaponFromUserAsync(int userId, int weaponId)
        {
            try
            {
                var userResponse = await _userService.RemoveWeaponFromUserByIdAsync(userId, weaponId);

                if (userResponse == null)
                {
                    return NotFound();
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpDelete]
        [Route("{requesterId}/friends/{receiverId}")]
        public async Task<IActionResult> RemoveFriendFromUserAsync(int requesterId, int receiverId)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != requesterId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var userResponse = await _userService.RemoveFriendFromUserByIdAsync(requesterId, receiverId);

                if (userResponse == null)
                {
                    return NotFound();
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var userResponses = await _userService.FindAllUsersAsync();

                if (userResponses.Count == 0)
                {
                    return NoContent();
                }

                return Ok(userResponses);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> FindUserByIdAsync(int userId)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != userId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var userResponse = await _userService.FindUserByIdAsync(userId);

                if (userResponse == null)
                {
                    return NotFound();
                }

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserRequest userRequest)
        {
            try
            {
                var userResponse = await _userService.CreateUserAsync(userRequest);

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPut]
        [Route("{userId}")]
        public async Task<IActionResult> UpdateUserByIdAsync(int userId, [FromBody] UserRequest userRequest)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != userId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var userResponse = await _userService.UpdateUserByIdAsync(userId, userRequest);

                if (userResponse == null)
                {
                    return NotFound();
                }
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> DeleteUserByIdAsync(int userId)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != userId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var userResponse = await _userService.DeleteUserByIdAsync(userId);

                if (userResponse == null)
                {
                    return NotFound();
                }
                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
