namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;

    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
        Task SeedDefaultUsersAsync();
        Task<bool> CreateUserAsync(string username, string password, string role); 
    }
}