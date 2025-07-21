namespace VendingMachine.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; } = string.Empty;
        public int Cost { get; set; } // In cents
        public int AmountAvailable { get; set; }
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public User? Seller { get; set; }
    }
}
