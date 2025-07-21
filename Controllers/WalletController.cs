using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.DTOs;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Buyer")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<DepositResponse>> Deposit(DepositRequest request)
        {
            if (request.Coins.Any(coin => !_walletService.IsValidCoin(coin)))
            {
                return BadRequest(new
                {
                    error = "Invalid coin denomination. Only 5, 10, 20, 50, and 100 cent coins are accepted."
                });
            }

            var userId = User.FindFirst("userId")?.Value;
            var response = await _walletService.DepositCoinsAsync(userId!, request.Coins);
            return Ok(response);
        }

        [HttpGet("balance")]
        public async Task<ActionResult<object>> GetBalance()
        {
            var userId = User.FindFirst("userId")?.Value;
            var balance = await _walletService.GetBalanceAsync(userId!);
            return Ok(new { balance, currency = "cents" });
        }

        [HttpPost("reset")]
        public async Task<ActionResult<object>> ResetWallet()
        {
            var userId = User.FindFirst("userId")?.Value;
            var returnedCoins = await _walletService.ResetWalletAsync(userId!);
            var returnedAmount = returnedCoins.Sum();

            return Ok(new
            {
                message = "Wallet reset successfully",
                returned_coins = returnedCoins,
                returned_amount = returnedAmount
            });
        }
    }
}
