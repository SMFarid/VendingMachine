using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public interface IPurchaseService
    {
        Task<PurchaseResponse> PurchaseProductAsync(int buyerId, PurchaseRequest request);
        Task<List<TransactionDto>> GetTransactionHistoryAsync(int userId, int limit = 50, int offset = 0);
    }
}
