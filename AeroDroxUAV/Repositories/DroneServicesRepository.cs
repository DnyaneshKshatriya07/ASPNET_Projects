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
            return await _context.DroneServices.ToListAsync();
        }

        public async Task<DroneServices?> GetByIdAsync(int id)
        {
            // *** FIX FOR EDIT/TRACKING CONFLICT ***
            // By making this read non-tracking, we allow the subsequent Update() call
            // in the API controller to attach the incoming entity without conflict.
            return await _context.DroneServices.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(DroneServices droneServices)
        {
            await _context.DroneServices.AddAsync(droneServices);
        }

        public void Update(DroneServices droneServices)
        {
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