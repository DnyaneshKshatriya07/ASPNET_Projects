namespace AeroDroxUAV.Services
{
    using AeroDroxUAV.Models;
    using AeroDroxUAV.Repositories;
    
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _userRepository.GetByUsernameAndPasswordAsync(username, password);
        }

        public async Task SeedDefaultUsersAsync()
        {
            if (!await _userRepository.HasUsersAsync())
            {
                await _userRepository.AddAsync(new User { Username = "admin", Password = "admin123", Role = "Admin" });
                await _userRepository.AddAsync(new User { Username = "user", Password = "user123", Role = "User" });
                await _userRepository.SaveChangesAsync();
            }
        }
        
        public async Task<bool> CreateUserAsync(string username, string password, string role)
        {
            // Check if the username is unique
            var existingUser = await _userRepository.GetByUsernameAsync(username); 
            if (existingUser != null)
            {
                return false; // User already exists
            }
            
            var newUser = new User 
            {
                Username = username,
                Password = password,
                Role = role
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}