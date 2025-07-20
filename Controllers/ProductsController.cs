using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VendingMachine.DTOs;
using VendingMachine.Services;

namespace VendingMachine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetProducts([FromQuery] bool availableOnly = false)
        {
            var products = await _productService.GetAllProductsAsync(availableOnly);
            return Ok(new { products, total = products.Count });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Policy = "SellerOnly")]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductRequest request)
        {
            if (request.Cost % 5 != 0)
                return BadRequest(new { error = "Product cost must be a multiple of 5 cents" });

            var userId = User.FindFirst("userId")?.Value;
            var product = await _productService.CreateProductAsync(request, userId!);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(string id, UpdateProductRequest request)
        {
            if (request.Cost.HasValue && request.Cost.Value % 5 != 0)
                return BadRequest(new { error = "Product cost must be a multiple of 5 cents" });

            var userId = User.FindFirst("userId")?.Value;
            var product = await _productService.UpdateProductAsync(id, request, userId!);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "SellerOnly")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var userId = User.FindFirst("userId")?.Value;
            var success = await _productService.DeleteProductAsync(id, userId!);
            if (!success) return NotFound();
            return Ok(new { message = "Product removed successfully", productId = id });
        }
    }
}
