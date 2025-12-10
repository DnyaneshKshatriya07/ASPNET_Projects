namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;
    using AeroDroxUAV.Repositories; // <-- This using statement is essential
    
    public class DroneServicesService : IDroneServicesService
    {
        // Correct field name: _droneServicesRepository
        private readonly IDroneServicesRepository _droneServicesRepository;

        public DroneServicesService(IDroneServicesRepository droneServicesRepository)
        {
            _droneServicesRepository = droneServicesRepository;
        }

        public async Task<IEnumerable<DroneServices>> GetAllDroneServicesAsync()
        {
            return await _droneServicesRepository.GetAllAsync();
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
            // *** FIX APPLIED HERE: Changed _droneServicesService to _droneServicesRepository ***
            var droneServices = await _droneServicesRepository.GetByIdAsync(id);
            
            if (droneServices != null)
            {
                _droneServicesRepository.Delete(droneServices);
                await _droneServicesRepository.SaveChangesAsync();
            }
        }
    }
}