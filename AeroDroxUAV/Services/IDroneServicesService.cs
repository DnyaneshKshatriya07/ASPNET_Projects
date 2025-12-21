namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;
    
    public interface IDroneServicesService
    {
        Task<IEnumerable<DroneServices>> GetAllDroneServicesAsync();
        Task<IEnumerable<DroneServices>> GetActiveDroneServicesAsync();
        Task<IEnumerable<DroneServices>> GetDroneServicesByCategoryAsync(string category);
        Task<IEnumerable<DroneServices>> GetFeaturedDroneServicesAsync();
        Task<DroneServices?> GetDroneServicesByIdAsync(int id);
        Task CreateDroneServicesAsync(DroneServices droneServices);
        Task UpdateDroneServicesAsync(DroneServices droneServices);
        Task DeleteDroneServicesAsync(int id);
        Task ToggleServiceStatusAsync(int id);
    }
}