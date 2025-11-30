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
            return await _context.Drones.FindAsync(id);
        }

        public async Task AddAsync(Drone drone)
        {
            await _context.Drones.AddAsync(drone);
        }

        public void Update(Drone drone)
        {
            _context.Drones.Update(drone);
        }

        public void Delete(Drone drone)
        {
            _context.Drones.Remove(drone);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}