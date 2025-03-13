using ShootyGameAPI.Database.Entities;
using ShootyGameAPI.DTOs;
using ShootyGameAPI.Repositorys;

namespace ShootyGameAPI.Services
{
    public interface IWeaponTypeService
    {
        Task<List<WeaponTypeResponse>> GetAllWeaponTypesAsync();
        Task<WeaponTypeResponse?> FindWeaponTypeByIdAsync(int weaponTypeId);
        Task<WeaponTypeResponse?> CreateWeaponTypeAsync(WeaponTypeRequest newWeaponType);
        Task<WeaponTypeResponse?> UpdateWeaponTypeAsync(int weaponTypeId, WeaponTypeRequest updatedWeaponType);
        Task<WeaponTypeResponse?> DeleteWeaponTypeAsync(int weaponTypeId);
    }

    public class WeaponTypeService : IWeaponTypeService
    {
        private readonly IWeaponTypeRepository _weaponTypeRepository;

        public WeaponTypeService(IWeaponTypeRepository weaponTypeRepository)
        {
            _weaponTypeRepository = weaponTypeRepository;
        }

        private WeaponTypeResponse MapWeaponTypeToResponse(WeaponType weaponType)
        {
            return new WeaponTypeResponse
            {
                WeaponTypeId = weaponType.WeaponTypeId,
                Name = weaponType.Name,
                EquipmentSlot = weaponType.EquipmentSlot
            };
        }

        private WeaponType MapWeaponTypeRequestToEntity(WeaponTypeRequest request)
        {
            return new WeaponType
            {
                Name = request.Name,
                EquipmentSlot = request.EquipmentSlot
            };
        }

        public async Task<List<WeaponTypeResponse>> GetAllWeaponTypesAsync()
        {
            var weaponTypes = await _weaponTypeRepository.GetAllWeaponTypesAsync();
            return weaponTypes.Select(MapWeaponTypeToResponse).ToList();
        }

        public async Task<WeaponTypeResponse?> FindWeaponTypeByIdAsync(int weaponTypeId)
        {
            var weaponType = await _weaponTypeRepository.FindWeaponTypeByIdAsync(weaponTypeId);

            if (weaponType == null)
            {
                return null;
            }

            return MapWeaponTypeToResponse(weaponType);
        }

        public async Task<WeaponTypeResponse?> CreateWeaponTypeAsync(WeaponTypeRequest newWeaponType)
        {
            var weaponType = MapWeaponTypeRequestToEntity(newWeaponType);
            var createdWeaponType = await _weaponTypeRepository.CreateWeaponTypeAsync(weaponType);

            if (createdWeaponType == null)
            {
                return null;
            }

            return MapWeaponTypeToResponse(createdWeaponType);
        }

        public async Task<WeaponTypeResponse?> UpdateWeaponTypeAsync(int weaponTypeId, WeaponTypeRequest updatedWeaponType)
        {
            var updatedEntity = MapWeaponTypeRequestToEntity(updatedWeaponType);
            var updatedWeaponTypeEntity = await _weaponTypeRepository.UpdateWeaponTypeByIdAsync(weaponTypeId, updatedEntity);

            if (updatedWeaponTypeEntity == null)
            {
                return null;
            }

            return MapWeaponTypeToResponse(updatedWeaponTypeEntity);
        }

        public async Task<WeaponTypeResponse?> DeleteWeaponTypeAsync(int weaponTypeId)
        {
            var deletedWeaponType = await _weaponTypeRepository.DeleteWeaponTypeByIdAsync(weaponTypeId);

            if (deletedWeaponType == null)
            {
                return null;
            }

            return MapWeaponTypeToResponse(deletedWeaponType);
        }
    }
}
