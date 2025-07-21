using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Models
{

    public class User
    {
        public int Id { get; set; } // Primary key
        public UserRole Role { get; set; }
        public int Balance { get; set; } // In cents
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // This should be hashed in a real application
    }

    public enum UserRole
    {
        Buyer,
        Seller
    }

}
