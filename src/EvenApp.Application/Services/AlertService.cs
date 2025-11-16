using EvenApp.Application.DTOs;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;

namespace EvenApp.Application.Services;

public class AlertService : IAlertService
{
    private readonly IAlertRepository _alertRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProductRepository _productRepository;

    public AlertService(
        IAlertRepository alertRepository,
        IInventoryRepository inventoryRepository,
        IProductRepository productRepository)
    {
        _alertRepository = alertRepository;
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
    }

    public async Task<AlertDto> CreateAlertAsync(CreateAlertRequest request)
    {
        var alertType = Enum.Parse<AlertType>(request.Type);
        var alert = new Alert
        {
            Type = alertType,
            ProductId = request.ProductId,
            Message = request.Message,
            IsResolved = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _alertRepository.CreateAsync(alert);
        var created = await _alertRepository.GetByIdAsync(id);

        return MapToDto(created!);
    }

    public async Task<AlertDto?> GetAlertByIdAsync(int id)
    {
        var alert = await _alertRepository.GetByIdAsync(id);
        return alert == null ? null : MapToDto(alert);
    }

    public async Task<IEnumerable<AlertDto>> GetActiveAlertsAsync()
    {
        var alerts = await _alertRepository.GetActiveAlertsAsync();
        return alerts.Select(MapToDto);
    }

    public async Task<IEnumerable<AlertDto>> GetAlertsByProductIdAsync(int productId)
    {
        var alerts = await _alertRepository.GetByProductIdAsync(productId);
        return alerts.Select(MapToDto);
    }

    public async Task<bool> ResolveAlertAsync(int alertId, int userId)
    {
        return await _alertRepository.ResolveAlertAsync(alertId, userId);
    }

    public async Task CheckReorderLevelsAsync()
    {
        var allInventory = await _inventoryRepository.GetAllAsync();
        
        foreach (var inventory in allInventory)
        {
            var product = await _productRepository.GetByIdAsync(inventory.ProductId);
            if (product == null) continue;

            if (inventory.Quantity <= product.ReorderLevel)
            {
                // Check if alert already exists
                var existingAlerts = await _alertRepository.GetByProductIdAsync(inventory.ProductId);
                var activeLowStockAlert = existingAlerts.FirstOrDefault(a => 
                    a.Type == AlertType.LowStock && !a.IsResolved);

                if (activeLowStockAlert == null)
                {
                    var alert = new Alert
                    {
                        Type = AlertType.LowStock,
                        ProductId = inventory.ProductId,
                        Message = $"Low stock alert: {product.Name} (SKU: {product.SKU}) has only {inventory.Quantity} units remaining. Reorder level: {product.ReorderLevel}",
                        IsResolved = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _alertRepository.CreateAsync(alert);
                }
            }
        }
    }

    private static AlertDto MapToDto(Alert alert)
    {
        return new AlertDto
        {
            Id = alert.Id,
            Type = alert.Type.ToString(),
            ProductId = alert.ProductId,
            ProductName = alert.Product?.Name,
            ProductSKU = alert.Product?.SKU,
            Message = alert.Message,
            IsResolved = alert.IsResolved,
            CreatedAt = alert.CreatedAt,
            ResolvedAt = alert.ResolvedAt
        };
    }
}

