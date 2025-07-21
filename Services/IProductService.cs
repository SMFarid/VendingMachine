using VendingMachine.DTOs;

namespace VendingMachine.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync(bool availableOnly = false);
        Task<ProductDto?> GetProductByIdAsync(string id);
        Task<ProductDto> CreateProductAsync(CreateProductRequest request, int sellerId);
        Task<ProductDto?> UpdateProductAsync(string id, UpdateProductRequest request, int sellerId);
        Task<bool> DeleteProductAsync(string id, int sellerId);
    }
}
