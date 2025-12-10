namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;
    using AeroDroxUAV.Repositories;
    
    public class DroneService : IDroneService
    {
        private readonly IDroneRepository _droneRepository;

        public DroneService(IDroneRepository droneRepository)
        {
            _droneRepository = droneRepository;
        }

        public async Task<IEnumerable<Drone>> GetAllDronesAsync()
        {
            return await _droneRepository.GetAllAsync();
        }

        public async Task<Drone?> GetDroneByIdAsync(int id)
        {
            // This call uses the corrected non-tracking repository method
            return await _droneRepository.GetByIdAsync(id);
        }

        public async Task CreateDroneAsync(Drone drone)
        {
            await _droneRepository.AddAsync(drone);
            await _droneRepository.SaveChangesAsync();
        }

        public async Task UpdateDroneAsync(Drone drone)
        {
            _droneRepository.Update(drone);
            await _droneRepository.SaveChangesAsync();
        }

        public async Task DeleteDroneAsync(int id)
        {
            // This call uses the corrected non-tracking repository method
            var drone = await _droneRepository.GetByIdAsync(id);
            if (drone != null)
            {
                _droneRepository.Delete(drone);
                await _droneRepository.SaveChangesAsync();
            }
        }
    }
}