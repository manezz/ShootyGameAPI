using Microsoft.AspNetCore.Mvc;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : Controller
    {
        private readonly IScoreService _scoreService;

        public ScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllScoresAsync()
        {
            try
            {
                var scoreResponses = await _scoreService.GetAllScoresAsync();

                if (scoreResponses.Count == 0)
                {
                    return NoContent();
                }

                return Ok(scoreResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpGet("{scoreId}")]
        public async Task<IActionResult> FindScoreByIdAsync(int scoreId)
        {
            try
            {
                var scoreResponse = await _scoreService.FindScoreByIdAsync(scoreId);

                if (scoreResponse == null)
                {
                    return NotFound();
                }

                return Ok(scoreResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.User, Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateScoreAsync([FromBody] ScoreRequest scoreRequest)
        {
            try
            {
                var currentUser = (UserResponse?)HttpContext.Items["User"];

                if (currentUser == null || currentUser.UserId != scoreRequest.UserId && currentUser.Role != Role.Admin)
                {
                    return Unauthorized(new { message = "Unauthorized" });
                }

                var scoreResponse = await _scoreService.CreateScoreAsync(scoreRequest);

                return Ok(scoreResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut("{scoreId}")]
        public async Task<IActionResult> UpdateScoreByIdAsync(int scoreId, [FromBody] ScoreRequest scoreRequest)
        {
            try
            {
                var scoreResponse = await _scoreService.UpdateScoreByIdAsync(scoreId, scoreRequest);

                if (scoreResponse == null)
                {
                    return NotFound();
                }

                return Ok(scoreResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{scoreId}")]
        public async Task<IActionResult> DeleteScoreByIdAsync(int scoreId)
        {
            try
            {
                var scoreResponse = await _scoreService.DeleteScoreByIdAsync(scoreId);

                if (scoreResponse == null)
                {
                    return NotFound();
                }

                return Ok(scoreResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
