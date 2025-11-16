using EvenApp.Application.DTOs;

namespace EvenApp.Application.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, int? userId);
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<IEnumerable<OrderDto>> GetOrdersBySupplierIdAsync(int supplierId);
    Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status);
    Task<OrderDto> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request);
}

