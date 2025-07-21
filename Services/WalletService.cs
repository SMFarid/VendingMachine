using VendingMachine.Data;
using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public class WalletService : IWalletService
    {
        private readonly VendingMachineContext _context;
        private readonly List<int> _validCoins = new() { 5, 10, 20, 50, 100 };

        public WalletService(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<DepositResponse> DepositCoinsAsync(int userId, List<int> coins)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found");

            var depositAmount = coins.Sum();
            user.Balance += depositAmount;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new DepositResponse
            {
                Message = "Coins deposited successfully",
                DepositedAmount = depositAmount,
                TotalBalance = user.Balance
            };
        }

        public async Task<int> GetBalanceAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.Balance ?? 0;
        }

        public async Task<List<int>> ResetWalletAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found");

            var returnedCoins = CalculateChange(user.Balance);
            user.Balance = 0;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return returnedCoins;
        }

        public bool IsValidCoin(int coin) => _validCoins.Contains(coin);

        public List<int> CalculateChange(int amount)
        {
            var change = new List<int>();
            var denominations = _validCoins.OrderByDescending(x => x).ToList();

            foreach (var denomination in denominations)
            {
                while (amount >= denomination)
                {
                    change.Add(denomination);
                    amount -= denomination;
                }
            }

            return change;
        }
    }
}
