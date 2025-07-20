using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Models
{

    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
        public int Deposit { get; set; } // In cents
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string Username
        {
            get => UserName ?? string.Empty;
            set => UserName = value;
        }
        public string Password
        {
            get => PasswordHash ?? string.Empty;
            set => PasswordHash = value;
        }
    }

    public enum UserRole
    {
        Buyer,
        Seller
    }

}
