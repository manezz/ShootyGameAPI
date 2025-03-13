using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPI.Services
{
    public interface IWeaponService
    {
        Task<List<WeaponResponse>> GetAllWeaponsAsync();
        Task<WeaponResponse?> FindWeaponByIdAsync(int weaponId);
        Task<WeaponResponse?> CreateWeaponAsync(WeaponRequest newWeapon);
        Task<WeaponResponse?> UpdateWeaponByIdAsync(int weaponId, WeaponRequest updatedWeapon);
        Task<WeaponResponse?> DeleteWeaponByIdAsync(int weaponId);
    }

    public class WeaponService : IWeaponService
    {
        private readonly IWeaponRepository _weaponRepository;
        private readonly IWeaponTypeRepository _weaponTypeRepository;

        public WeaponService(IWeaponRepository weaponRepository, IWeaponTypeRepository weaponTypeRepository)
        {
            _weaponRepository = weaponRepository;
            _weaponTypeRepository = weaponTypeRepository;
        }

        private WeaponResponse MapWeaponToWeaponResponse(Weapon weapon)
        {
            return new WeaponResponse
            {
                WeaponId = weapon.WeaponId,
                Name = weapon.Name,
                Price = weapon.Price,
                ReloadSpeed = weapon.ReloadSpeed,
                MagSize = weapon.MagSize,
                FireRate = weapon.FireRate,
                FireMode = weapon.FireMode,
                WeaponType = new Weapon_WeaponTypeResponse
                {
                    WeaponTypeId = weapon.WeaponType.WeaponTypeId,
                    Name = weapon.WeaponType.Name,
                    EquipmentSlot = weapon.WeaponType.EquipmentSlot
                }
            };
        }

        private Weapon MapWeaponRequestToWeapon(WeaponRequest weaponRequest)
        {
            return new Weapon
            {
                Name = weaponRequest.Name,
                Price = weaponRequest.Price,
                ReloadSpeed = weaponRequest.ReloadSpeed,
                MagSize = weaponRequest.MagSize,
                FireRate = weaponRequest.FireRate,
                FireMode = weaponRequest.FireMode,
                WeaponTypeId = weaponRequest.WeaponTypeId
            };
        }

        public async Task<List<WeaponResponse>> GetAllWeaponsAsync()
        {
            var weapons = await _weaponRepository.GetAllWeaponsAsync();
            return weapons.Select(MapWeaponToWeaponResponse).ToList();
        }

        public async Task<WeaponResponse?> FindWeaponByIdAsync(int weaponId)
        {
            var weapon = await _weaponRepository.FindWeaponByIdAsync(weaponId);

            if (weapon == null)
            {
                return null;
            }

            return MapWeaponToWeaponResponse(weapon);
        }

        public async Task<WeaponResponse?> CreateWeaponAsync(WeaponRequest newWeapon)
        {
            var user = await _weaponRepository.CreateWeaponAsync(MapWeaponRequestToWeapon(newWeapon));

            if (user == null)
            {
                return null;
            }

            return MapWeaponToWeaponResponse(user);
        }

        public async Task<WeaponResponse?> UpdateWeaponByIdAsync(int weaponId, WeaponRequest updatedWeapon)
        {
            var user = await _weaponRepository.UpdateWeaponByIdAsync(weaponId, MapWeaponRequestToWeapon(updatedWeapon));

            if (user == null)
            {
                return null;
            }

            return MapWeaponToWeaponResponse(user);
        }

        public async Task<WeaponResponse?> DeleteWeaponByIdAsync(int weaponId)
        {
            var deletedWeapon = await _weaponRepository.DeleteWeaponByIdAsync(weaponId);

            if (deletedWeapon == null)
            {
                return null;
            }

            return MapWeaponToWeaponResponse(deletedWeapon);
        }
    }
}
