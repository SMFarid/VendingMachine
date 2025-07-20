using VendingMachine.Models;

namespace VendingMachine.Services
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}
