using Microsoft.EntityFrameworkCore;
using VendingMachine.Data;
using VendingMachine.DTOs;
using VendingMachine.Models;

namespace VendingMachine.Services
{
    public class ProductService : IProductService
    {
        private readonly VendingMachineContext _context;

        public ProductService(VendingMachineContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync(bool availableOnly = false)
        {
            var query = _context.Products.AsQueryable();

            if (availableOnly)
                query = query.Where(p => p.AmountAvailable > 0);

            return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Cost = p.Cost,
                AmountAvailable = p.AmountAvailable,
                SellerId = p.SellerId,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                SellerId = product.SellerId,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, int sellerId)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                Cost = request.Cost,
                AmountAvailable = request.AmountAvailable,
                SellerId = sellerId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                SellerId = product.SellerId,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<ProductDto?> UpdateProductAsync(string id, UpdateProductRequest request, int sellerId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.SellerId != sellerId) return null;

            if (!string.IsNullOrWhiteSpace(request.ProductName))
                product.ProductName = request.ProductName;
            if (request.Cost.HasValue)
                product.Cost = request.Cost.Value;
            if (request.AmountAvailable.HasValue)
                product.AmountAvailable = request.AmountAvailable.Value;

            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Cost = product.Cost,
                AmountAvailable = product.AmountAvailable,
                SellerId = product.SellerId,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<bool> DeleteProductAsync(string id, int sellerId)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null || product.SellerId != sellerId) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
