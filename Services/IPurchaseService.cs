using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public interface IPurchaseService
    {
        Task<PurchaseResponse> PurchaseProductAsync(string buyerId, PurchaseRequest request);
        //Task<List<TransactionDto>> GetTransactionHistoryAsync(string userId, int limit = 50, int offset = 0);
    }
}
