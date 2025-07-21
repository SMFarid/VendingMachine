using VendingMachine.Models;

namespace VendingMachine.DTOs
{
    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int? Role { get; set; } // "Buyer" or "Seller"
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
        public int ExpiresIn { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
