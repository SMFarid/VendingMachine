using Microsoft.AspNetCore.Identity;
using VendingMachine.Models;

namespace VendingMachine.Data
{
    public static class SeedData
    {
        public static async Task Initialize(VendingMachineContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.Users.Any()) return;

            // Create test users
            var seller = new ApplicationUser
            {
                UserName = "seller1",
                Email = "seller1@example.com",
                Role = UserRole.Seller
            };
            await userManager.CreateAsync(seller, "Seller123!");

            var buyer = new ApplicationUser
            {
                UserName = "buyer1",
                Email = "buyer1@example.com",
                Role = UserRole.Buyer,
                Balance = 0
            };
            await userManager.CreateAsync(buyer, "Buyer123!");

            await context.SaveChangesAsync();

            // Create test products
            var products = new List<Product>
        {
            new Product
            {
                ProductName = "Coca Cola",
                Cost = 150,
                AmountAvailable = 10,
                SellerId = seller.Id
            },
            new Product
            {
                ProductName = "Pepsi",
                Cost = 140,
                AmountAvailable = 8,
                SellerId = seller.Id
            },
            new Product
            {
                ProductName = "Water",
                Cost = 100,
                AmountAvailable = 15,
                SellerId = seller.Id
            }
        };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
