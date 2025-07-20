using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Models
{

    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
        public int Balance { get; set; } // In cents
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum UserRole
    {
        Buyer,
        Seller
    }

}
