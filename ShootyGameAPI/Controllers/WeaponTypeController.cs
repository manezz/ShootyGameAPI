using Microsoft.AspNetCore.Mvc;
using ShootyGameAPI.Authorization;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Helpers;
using ShootyGameAPI.Services;

namespace ShootyGameAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponTypeController : Controller
    {
        private readonly IWeaponTypeService _weaponTypeService;

        public WeaponTypeController(IWeaponTypeService weaponTypeService)
        {
            _weaponTypeService = weaponTypeService;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllWeaponTypesAsync()
        {
            try
            {
                var weaponTypeResponses = await _weaponTypeService.GetAllWeaponTypesAsync();

                if (weaponTypeResponses.Count == 0)
                {
                    return NoContent();
                }

                return Ok(weaponTypeResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet("{weponTypeId}")]
        public async Task<IActionResult> FindWeaponTypeByIdAsync(int weponTypeId)
        {
            try
            {
                var weaponTypeResponse = await _weaponTypeService.FindWeaponTypeByIdAsync(weponTypeId);

                if (weaponTypeResponse == null)
                {
                    return NotFound();
                }

                return Ok(weaponTypeResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateWeaponTypeAsync([FromBody] WeaponTypeRequest weaponTypeRequest)
        {
            try
            {
                var weaponTypeResponse = await _weaponTypeService.CreateWeaponTypeAsync(weaponTypeRequest);

                return Ok(weaponTypeResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut("{weponTypeId}")]
        public async Task<IActionResult> UpdateWeaponTypeByIdAsync(int weponTypeId, [FromBody] WeaponTypeRequest weaponTypeRequest)
        {
            try
            {
                var weaponTypeResponse = await _weaponTypeService.UpdateWeaponTypeAsync(weponTypeId, weaponTypeRequest);

                if (weaponTypeResponse == null)
                {
                    return NotFound();
                }

                return Ok(weaponTypeResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{weponTypeId}")]
        public async Task<IActionResult> DeleteWeaponTypeByIdAsync(int weponTypeId)
        {
            try
            {
                var weaponTypeResponse = await _weaponTypeService.DeleteWeaponTypeAsync(weponTypeId);

                if (weaponTypeResponse == null)
                {
                    return NotFound();
                }

                return Ok(weaponTypeResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
