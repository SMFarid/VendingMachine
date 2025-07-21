namespace VendingMachine.DTOs
{
    public class CreateProductRequest
    {
        public string ProductName { get; set; } = string.Empty;
        public int Cost { get; set; }
        public int AmountAvailable { get; set; }
    }

    public class UpdateProductRequest
    {
        public string? ProductName { get; set; }
        public int? Cost { get; set; }
        public int? AmountAvailable { get; set; }
    }

    public class ProductDto
    {
        public string Id { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Cost { get; set; }
        public int AmountAvailable { get; set; }
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
