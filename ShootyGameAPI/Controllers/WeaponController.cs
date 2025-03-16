using Microsoft.AspNetCore.Mvc;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : Controller
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpGet]
        public async Task<IActionResult> GetAllWeaponsAsync()
        {
            try
            {
                var weaponResponses = await _weaponService.GetAllWeaponsAsync();

                if (weaponResponses.Count == 0)
                {
                    return NoContent();
                }

                return Ok(weaponResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpGet("{weaponId}")]
        public async Task<IActionResult> FindWeaponByIdAsync(int weaponId)
        {
            try
            {
                var weaponResponse = await _weaponService.FindWeaponByIdAsync(weaponId);

                if (weaponResponse == null)
                {
                    return NotFound();
                }

                return Ok(weaponResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateWeaponAsync([FromBody] WeaponRequest weaponRequest)
        {
            try
            {
                var weaponResponse = await _weaponService.CreateWeaponAsync(weaponRequest);

                return Ok(weaponResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut("{weaponId}")]
        public async Task<IActionResult> UpdateWeaponByIdAsync(int weaponId, [FromBody] WeaponRequest weaponRequest)
        {
            try
            {
                var weaponResponse = await _weaponService.UpdateWeaponByIdAsync(weaponId, weaponRequest);

                if (weaponResponse == null)
                {
                    return NotFound();
                }

                return Ok(weaponResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{weaponId}")]
        public async Task<IActionResult> DeleteWeaponByIdAsync(int weaponId)
        {
            try
            {
                var weaponResponse = await _weaponService.DeleteWeaponByIdAsync(weaponId);

                if (weaponResponse == null)
                {
                    return NotFound();
                }

                return Ok(weaponResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
