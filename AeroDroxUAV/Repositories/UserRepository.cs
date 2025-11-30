namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Data;
    using AeroDroxUAV.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
        
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> HasUsersAsync()
        {
            return await _context.Users.AnyAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }
        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}