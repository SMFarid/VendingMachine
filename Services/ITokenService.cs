using VendingMachine.Models;

namespace VendingMachine.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
