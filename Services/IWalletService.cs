using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public interface IWalletService
    {
        Task<DepositResponse> DepositCoinsAsync(int userId, List<int> coins);
        Task<int> GetBalanceAsync(int userId);
        Task<List<int>> ResetWalletAsync(int userId);
        bool IsValidCoin(int coin);
        List<int> CalculateChange(int amount);
    }
}
