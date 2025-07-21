using System.ComponentModel.DataAnnotations;
using VendingMachine.Models;

namespace VendingMachine.DTOs
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }
        public int? Role { get; set; }
        public int? Balance { get; set; } // In cents
        public string? Password { get; set; } 
        
    }
}
