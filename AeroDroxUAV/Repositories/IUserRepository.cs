namespace AeroDroxUAV.Repositories
{
    using AeroDroxUAV.Models;
    
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAndPasswordAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username); // NEW: For uniqueness check
        Task<bool> HasUsersAsync();
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}