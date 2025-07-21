using Microsoft.EntityFrameworkCore;
using VendingMachine.Data;
using VendingMachine.DTOs;
using VendingMachine.Models;

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

        public async Task<PurchaseResponse> PurchaseProductAsync(int buyerId, PurchaseRequest request)
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

            // Create transaction record
            var transaction = new Transaction
            {
                ProductId = product.Id,
                ProductName = product.ProductName,
                Quantity = request.Quantity,
                UnitCost = product.Cost,
                TotalCost = totalCost,
                BuyerId = buyerId,
                SellerId = product.SellerId
            };

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            // Calculate change (not deducted from balance, just informational)
            var change = _walletService.CalculateChange(buyer.Balance);

            return new PurchaseResponse
            {
                Message = "Purchase successful",
                Transaction = new TransactionDto
                {
                    Id = transaction.Id,
                    ProductId = transaction.ProductId,
                    ProductName = transaction.ProductName,
                    Quantity = transaction.Quantity,
                    UnitCost = transaction.UnitCost,
                    TotalCost = transaction.TotalCost,
                    BuyerId = transaction.BuyerId,
                    SellerId = transaction.SellerId,
                    Timestamp = transaction.Timestamp
                },
                Change = change,
                RemainingBalance = buyer.Balance
            };
        }

        //Can be used to retrieve the transaction history for a user
        //Can be added as an endpoint if needed. Will only be accessible to Sellers. Will show each Seller's sales history.
        public async Task<List<TransactionDto>> GetTransactionHistoryAsync(int userId, int limit = 50, int offset = 0)
        {
            return await _context.Transactions
                .Where(t => t.SellerId == userId)
                .OrderByDescending(t => t.Timestamp)
                .Skip(offset)
                .Take(limit)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    ProductId = t.ProductId,
                    ProductName = t.ProductName,
                    Quantity = t.Quantity,
                    UnitCost = t.UnitCost,
                    TotalCost = t.TotalCost,
                    BuyerId = t.BuyerId,
                    SellerId = t.SellerId,
                    Timestamp = t.Timestamp
                })
                .ToListAsync();
        }
    }
}
