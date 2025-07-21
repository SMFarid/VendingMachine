using VendingMachine.Data;
using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly VendingMachineContext _context;
        private readonly IWalletService _walletService;

        public PurchaseService(VendingMachineContext context, IWalletService walletService)
        {
            _context = context;
            _walletService = walletService;
        }

        public async Task<PurchaseResponse> PurchaseProductAsync(string buyerId, PurchaseRequest request)
        {
            var buyer = await _context.Users.FindAsync(buyerId);
            var product = await _context.Products.FindAsync(request.ProductId);

            if (buyer == null) throw new InvalidOperationException("Buyer not found");
            if (product == null) throw new InvalidOperationException("Product not found");
            if (product.AmountAvailable < request.Quantity)
                throw new InvalidOperationException("Insufficient stock");

            var totalCost = product.Cost * request.Quantity;
            if (buyer.Balance < totalCost)
                throw new InvalidOperationException("Insufficient funds");

            // Update buyer balance and product stock
            buyer.Balance -= totalCost;
            product.AmountAvailable -= request.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            buyer.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Calculate change (not deducted from balance, just informational)
            var change = _walletService.CalculateChange(buyer.Balance);

            return new PurchaseResponse
            {
                Message = "Purchase successful",

                Change = change,
                RemainingBalance = buyer.Balance
            };
        }

    }
}
