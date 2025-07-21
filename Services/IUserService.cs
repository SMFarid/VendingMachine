using VendingMachine.DTOs;
using VendingMachine.Models;

namespace VendingMachine.Services
{

    public interface IUserService
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(UserDTO user); 
        Task<bool> DeleteUserAsync(int userId);
    }

}
