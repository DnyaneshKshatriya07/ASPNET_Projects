namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;
    
    public interface IDroneServicesService
    {
        Task<IEnumerable<DroneServices>> GetAllDroneServicesAsync();
        Task<DroneServices?> GetDroneServicesByIdAsync(int id);
        Task CreateDroneServicesAsync(DroneServices droneServices);
        Task UpdateDroneServicesAsync(DroneServices droneServices);
        Task DeleteDroneServicesAsync(int id);
    }
}