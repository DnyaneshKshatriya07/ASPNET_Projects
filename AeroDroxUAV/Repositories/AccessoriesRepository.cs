namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Data;
    using AeroDroxUAV.Models;
    using Microsoft.EntityFrameworkCore;

    public class AccessoriesRepository : IAccessoriesRepository
    {
        private readonly AppDbContext _context;

        public AccessoriesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Accessories>> GetAllAsync()
        {
            return await _context.Accessories.ToListAsync();
        }

        public async Task<Accessories?> GetByIdAsync(int id)
        {
            // The AsNoTracking() is good practice for entities only used for view/edit forms
            return await _context.Accessories.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddAsync(Accessories accessories) // Corrected parameter name
        {
            await _context.Accessories.AddAsync(accessories);
        }

        public void Update(Accessories accessories) // Corrected method signature to match interface
        {
            _context.Accessories.Update(accessories);
        }

        public void Delete(Accessories accessories)
        {
            _context.Accessories.Remove(accessories);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // NEW METHOD: Get accessories for homepage
        public async Task<IEnumerable<Accessories>> GetHomepageAccessoriesAsync()
        {
            return await _context.Accessories
                .Where(a => a.ShowOnHomepage)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}