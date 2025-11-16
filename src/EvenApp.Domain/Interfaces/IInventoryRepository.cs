using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface IInventoryRepository : IRepository<InventoryItem>
{
    Task<InventoryItem?> GetByProductIdAsync(int productId);
    Task<IEnumerable<InventoryItem>> GetByLocationAsync(string location);
    Task<bool> UpdateQuantityAsync(int productId, int quantity, int? userId);
    Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold);
}

