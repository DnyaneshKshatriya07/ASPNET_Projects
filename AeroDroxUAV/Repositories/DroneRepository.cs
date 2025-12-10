namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Data;
    using AeroDroxUAV.Models;
    using Microsoft.EntityFrameworkCore;

    public class DroneRepository : IDroneRepository
    {
        private readonly AppDbContext _context;

        public DroneRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drone>> GetAllAsync()
        {
            return await _context.Drones.ToListAsync();
        }

        public async Task<Drone?> GetByIdAsync(int id)
        {
            // *** FIX FOR EDIT/TRACKING CONFLICT ***
            // Change from Find(id) or FirstOrDefaultAsync() to use AsNoTracking() 
            // to ensure the entity retrieved for existence checks in PUT/Edit methods 
            // is NOT tracked by EF Core. This prevents the "already tracked" error.
            return await _context.Drones.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Drone drone)
        {
            await _context.Drones.AddAsync(drone);
        }

        public void Update(Drone drone)
        {
            // This is now safe because the GetByIdAsync call in the controller is non-tracking.
            _context.Drones.Update(drone);
        }

        public void Delete(Drone drone)
        {
            // Note: If the incoming 'drone' object here is the result of a non-tracking query, 
            // EF Core will track it first, then remove it. This is usually fine.
            _context.Drones.Remove(drone);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}