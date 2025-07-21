using VendingMachine.Models;

namespace VendingMachine.Services
{

    public interface IUserService
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user); // For completeness
        Task<bool> DeleteUserAsync(int userId); // For completeness
    }

}
