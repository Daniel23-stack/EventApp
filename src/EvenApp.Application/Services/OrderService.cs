using EvenApp.Application.DTOs;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ISupplierRepository supplierRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _supplierRepository = supplierRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request, int? userId)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
        if (supplier == null)
        {
            throw new Exception("Supplier not found");
        }

        decimal totalAmount = 0;
        var orderItems = new List<OrderItem>();

        foreach (var itemRequest in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemRequest.ProductId);
            if (product == null)
            {
                throw new Exception($"Product with ID {itemRequest.ProductId} not found");
            }

            var itemTotal = itemRequest.Quantity * itemRequest.UnitPrice;
            totalAmount += itemTotal;

            orderItems.Add(new OrderItem
            {
                ProductId = itemRequest.ProductId,
                Quantity = itemRequest.Quantity,
                UnitPrice = itemRequest.UnitPrice,
                TotalPrice = itemTotal,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        var order = new Order
        {
            SupplierId = request.SupplierId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            CreatedBy = userId,
            Items = orderItems,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Create order using repository
        // Note: This is a simplified version. In a real implementation, you might need
        // a more complex order creation process that handles order items separately
        var orderId = await _orderRepository.CreateAsync(order);
        
        // For now, we'll need to handle order items separately
        // This would typically be done in the repository or through a separate service
        // For this implementation, we'll return the order after creation
        return await GetOrderByIdAsync(orderId);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : MapToDto(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersBySupplierIdAsync(int supplierId)
    {
        var orders = await _orderRepository.GetBySupplierIdAsync(supplierId);
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status)
    {
        var orderStatus = Enum.Parse<OrderStatus>(status);
        var orders = await _orderRepository.GetByStatusAsync(orderStatus);
        return orders.Select(MapToDto);
    }

    public async Task<OrderDto> UpdateOrderStatusAsync(int id, UpdateOrderStatusRequest request)
    {
        var orderStatus = Enum.Parse<OrderStatus>(request.Status);
        await _orderRepository.UpdateStatusAsync(id, orderStatus);
        var updated = await _orderRepository.GetByIdAsync(id);
        return MapToDto(updated!);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            SupplierId = order.SupplierId,
            SupplierName = order.Supplier?.Name,
            OrderDate = order.OrderDate,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            Items = order.Items.Select(item => new OrderItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name,
                ProductSKU = item.Product?.SKU,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            }).ToList(),
            CreatedAt = order.CreatedAt
        };
    }
}

