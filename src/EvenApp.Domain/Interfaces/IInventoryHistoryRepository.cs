using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface IInventoryHistoryRepository
{
    Task<int> CreateAsync(InventoryHistory history);
    Task<IEnumerable<InventoryHistory>> GetByProductIdAsync(int productId);
    Task<IEnumerable<InventoryHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<InventoryHistory>> GetByChangeTypeAsync(ChangeType changeType);
}

