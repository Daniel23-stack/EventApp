using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetBySupplierIdAsync(int supplierId);
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);
}

