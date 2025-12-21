namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;
    using AeroDroxUAV.Repositories;
    
    public class DroneServicesService : IDroneServicesService
    {
        private readonly IDroneServicesRepository _droneServicesRepository;

        public DroneServicesService(IDroneServicesRepository droneServicesRepository)
        {
            _droneServicesRepository = droneServicesRepository;
        }

        public async Task<IEnumerable<DroneServices>> GetAllDroneServicesAsync()
        {
            return await _droneServicesRepository.GetAllAsync();
        }

        public async Task<IEnumerable<DroneServices>> GetActiveDroneServicesAsync()
        {
            return await _droneServicesRepository.GetActiveServicesAsync();
        }

        public async Task<IEnumerable<DroneServices>> GetDroneServicesByCategoryAsync(string category)
        {
            return await _droneServicesRepository.GetByCategoryAsync(category);
        }

        public async Task<IEnumerable<DroneServices>> GetFeaturedDroneServicesAsync()
        {
            return await _droneServicesRepository.GetFeaturedServicesAsync();
        }

        public async Task<DroneServices?> GetDroneServicesByIdAsync(int id)
        {
            return await _droneServicesRepository.GetByIdAsync(id);
        }

        public async Task CreateDroneServicesAsync(DroneServices droneServices)
        {
            await _droneServicesRepository.AddAsync(droneServices);
            await _droneServicesRepository.SaveChangesAsync();
        }

        public async Task UpdateDroneServicesAsync(DroneServices droneServices)
        {
            _droneServicesRepository.Update(droneServices);
            await _droneServicesRepository.SaveChangesAsync();
        }

        public async Task DeleteDroneServicesAsync(int id)
        {
            var droneServices = await _droneServicesRepository.GetByIdAsync(id);
            if (droneServices != null)
            {
                _droneServicesRepository.Delete(droneServices);
                await _droneServicesRepository.SaveChangesAsync();
            }
        }

        public async Task ToggleServiceStatusAsync(int id)
        {
            var droneServices = await _droneServicesRepository.GetByIdAsync(id);
            if (droneServices != null)
            {
                droneServices.IsActive = !droneServices.IsActive;
                droneServices.UpdatedAt = DateTime.UtcNow;
                _droneServicesRepository.Update(droneServices);
                await _droneServicesRepository.SaveChangesAsync();
            }
        }
    }
}