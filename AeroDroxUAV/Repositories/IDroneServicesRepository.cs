namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Models;
    
    public interface IDroneServicesRepository
    {
        Task<IEnumerable<DroneServices>> GetAllAsync();
        // CHANGED: Removed underscores from parameter names for consistency
        Task<DroneServices?> GetByIdAsync(int id);
        Task AddAsync(DroneServices droneService);
        void Delete(DroneServices droneService);
        void Update(DroneServices droneService);
        Task SaveChangesAsync();
    }
}