namespace VendingMachine.DTOs
{
    public class DepositRequest
    {
        public List<int> Coins { get; set; } = new();
    }

    public class DepositResponse
    {
        public string Message { get; set; } = string.Empty;
        public int DepositedAmount { get; set; }
        public int TotalBalance { get; set; }
    }

    public class PurchaseRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    public class PurchaseResponse
    {
        public string Message { get; set; } = string.Empty;
        public TransactionDto Transaction { get; set; } = new();
        public List<int> Change { get; set; } = new();
        public int RemainingBalance { get; set; }
    }

    public class TransactionDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitCost { get; set; }
        public int TotalCost { get; set; }
        public int BuyerId { get; set; } 
        public int SellerId { get; set; } 
        public DateTime Timestamp { get; set; }
    }
}
