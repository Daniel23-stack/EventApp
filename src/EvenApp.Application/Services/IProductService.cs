using EvenApp.Application.DTOs;

namespace EvenApp.Application.Services;

public interface IProductService
{
    Task<ProductDto> CreateProductAsync(CreateProductRequest request);
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto?> GetProductBySKUAsync(string sku);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
    Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category);
    Task<ProductDto> UpdateProductAsync(int id, UpdateProductRequest request);
    Task<bool> DeleteProductAsync(int id);
}

