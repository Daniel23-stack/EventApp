using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<Product?> GetBySKUAsync(string sku);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<bool> SKUExistsAsync(string sku);
}

