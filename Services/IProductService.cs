using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync(bool availableOnly = false);
        Task<ProductDto?> GetProductByIdAsync(string id);
        Task<ProductDto> CreateProductAsync(CreateProductRequest request, string sellerId);
        Task<ProductDto?> UpdateProductAsync(string id, UpdateProductRequest request, string sellerId);
        Task<bool> DeleteProductAsync(string id, string sellerId);
    }
}
