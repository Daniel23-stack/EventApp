using EvenApp.Application.DTOs;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;

namespace EvenApp.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request)
    {
        if (await _productRepository.SKUExistsAsync(request.SKU))
        {
            throw new Exception("SKU already exists");
        }

        var product = new Product
        {
            SKU = request.SKU,
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            UnitPrice = request.UnitPrice,
            ReorderLevel = request.ReorderLevel,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _productRepository.CreateAsync(product);
        var createdProduct = await _productRepository.GetByIdAsync(id);

        return MapToDto(createdProduct!);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product == null ? null : MapToDto(product);
    }

    public async Task<ProductDto?> GetProductBySKUAsync(string sku)
    {
        var product = await _productRepository.GetBySKUAsync(sku);
        return product == null ? null : MapToDto(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
    {
        var products = await _productRepository.SearchAsync(searchTerm);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(string category)
    {
        var products = await _productRepository.GetByCategoryAsync(category);
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new Exception("Product not found");
        }

        product.Name = request.Name;
        product.Description = request.Description;
        product.Category = request.Category;
        product.UnitPrice = request.UnitPrice;
        product.ReorderLevel = request.ReorderLevel;
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);
        var updatedProduct = await _productRepository.GetByIdAsync(id);

        return MapToDto(updatedProduct!);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            SKU = product.SKU,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            UnitPrice = product.UnitPrice,
            ReorderLevel = product.ReorderLevel,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }
}

