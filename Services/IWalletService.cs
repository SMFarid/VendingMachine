using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public interface IWalletService
    {
        Task<DepositResponse> DepositCoinsAsync(string userId, List<int> coins);
        Task<int> GetBalanceAsync(string userId);
        Task<List<int>> ResetWalletAsync(string userId);
        bool IsValidCoin(int coin);
        List<int> CalculateChange(int amount);
    }
}
