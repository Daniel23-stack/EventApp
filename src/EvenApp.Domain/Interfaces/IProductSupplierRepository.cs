using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface IProductSupplierRepository
{
    Task<int> CreateAsync(ProductSupplier productSupplier);
    Task<IEnumerable<ProductSupplier>> GetByProductIdAsync(int productId);
    Task<IEnumerable<ProductSupplier>> GetBySupplierIdAsync(int supplierId);
    Task<bool> DeleteAsync(int productId, int supplierId);
}

