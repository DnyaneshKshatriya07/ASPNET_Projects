namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Models;
    
    public interface IDroneServicesRepository
    {
        Task<IEnumerable<DroneServices>> GetAllAsync();
        Task<IEnumerable<DroneServices>> GetActiveServicesAsync();
        Task<IEnumerable<DroneServices>> GetByCategoryAsync(string category);
        Task<IEnumerable<DroneServices>> GetFeaturedServicesAsync();
        Task<DroneServices?> GetByIdAsync(int id);
        Task AddAsync(DroneServices droneService);
        void Delete(DroneServices droneService);
        void Update(DroneServices droneService);
        Task SaveChangesAsync();
    }
}