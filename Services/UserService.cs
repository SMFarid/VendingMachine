using Azure.Core;
using VendingMachine.Data;
using VendingMachine.DTOs;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly VendingMachineContext _context;

        // In a real application, this would be a database context or repository
        private static List<User> _users = new List<User>
        {
            // Pre-seed some users for testing
            new User { Id = 1, UserName = "seller", Password = BCrypt.Net.BCrypt.HashPassword("sellerpass"), Role = UserRole.Seller },
            new User { Id = 2, UserName = "buyer", Password = BCrypt.Net.BCrypt.HashPassword("buyerpass"), Role = UserRole.Buyer }
        };
        private static int _nextUserId = 3; // For new registrations

        public UserService(ILogger<UserService> logger, VendingMachineContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            _logger.LogInformation("UserService: Attempting to get user by username: {Username}", username);
            // Simulate async operation
            await Task.Delay(10);
            return _context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
        }

        public async Task<User> AddUserAsync(User user)
        {
            _logger.LogInformation("UserService: Attempting to add new user: {Username}", user.UserName);
            // Simulate async operation
            await Task.Delay(10);
            //user.Id = _nextUserId++;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("UserService: User {Username} added with ID: {UserId}", user.UserName, user.Id);
            return user;
        }

        
        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            _logger.LogInformation("UserService: Attempting to update user with ID: {UserId}", user.Id);
            // Simulate async operation
            await Task.Delay(10);
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                _logger.LogWarning("UserService: Update failed, user with ID {UserId} not found.", user.Id);
                return false;
            }
            
            if (user.Role.HasValue)
                existingUser.Role = (UserRole)user.Role.Value;

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

             await _context.SaveChangesAsync();
            _logger.LogInformation("UserService: User with ID {UserId} updated successfully.", user.Id);
            return true;
        }

        
        public async Task<bool> DeleteUserAsync(int userId)
        {
            _logger.LogInformation("UserService: Attempting to delete user with ID: {UserId}", userId);
            // Simulate async operation
            await Task.Delay(10);
            var userToRemove = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (userToRemove == null)
            {
                _logger.LogWarning("UserService: Delete failed, user with ID {UserId} not found.", userId);
                return false;
            }
            _context.Users.Remove(userToRemove);
            await _context.SaveChangesAsync();
            _logger.LogInformation("UserService: User with ID {UserId} deleted successfully.", userId);
            return true;
        }
    }
}
