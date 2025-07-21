namespace VendingMachine.Models
{
    public class Transaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int UnitCost { get; set; } // In cents
        public int TotalCost { get; set; } // In cents
        public int BuyerId { get; set; } 
        public int SellerId { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public User? Buyer { get; set; }
        public User? Seller { get; set; }
        public Product? Product { get; set; }
    }
}
