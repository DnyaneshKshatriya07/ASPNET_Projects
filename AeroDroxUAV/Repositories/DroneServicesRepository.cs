namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Data;
    using AeroDroxUAV.Models;
    using Microsoft.EntityFrameworkCore;

    public class DroneServicesRepository : IDroneServicesRepository
    {
        private readonly AppDbContext _context;

        public DroneServicesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DroneServices>> GetAllAsync()
        {
            return await _context.DroneServices
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DroneServices>> GetActiveServicesAsync()
        {
            return await _context.DroneServices
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DroneServices>> GetByCategoryAsync(string category)
        {
            return await _context.DroneServices
                .Where(s => s.Category == category && s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DroneServices>> GetFeaturedServicesAsync()
        {
            return await _context.DroneServices
                .Where(s => s.IsActive)
                .OrderByDescending(s => s.CreatedAt)
                .Take(8)
                .ToListAsync();
        }

        public async Task<DroneServices?> GetByIdAsync(int id)
        {
            return await _context.DroneServices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(DroneServices droneServices)
        {
            droneServices.CreatedAt = DateTime.UtcNow;
            await _context.DroneServices.AddAsync(droneServices);
        }

        public void Update(DroneServices droneServices)
        {
            droneServices.UpdatedAt = DateTime.UtcNow;
            _context.DroneServices.Update(droneServices);
        }

        public void Delete(DroneServices droneServices)
        {
            _context.DroneServices.Remove(droneServices);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}