using EvenApp.Application.DTOs;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;

namespace EvenApp.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IInventoryHistoryRepository _historyRepository;

    public InventoryService(
        IInventoryRepository inventoryRepository,
        IProductRepository productRepository,
        IInventoryHistoryRepository historyRepository)
    {
        _inventoryRepository = inventoryRepository;
        _productRepository = productRepository;
        _historyRepository = historyRepository;
    }

    public async Task<InventoryDto> UpdateInventoryAsync(UpdateInventoryRequest request, int? userId)
    {
        var existing = await _inventoryRepository.GetByProductIdAsync(request.ProductId);
        var previousQuantity = existing?.Quantity ?? 0;

        if (existing == null)
        {
            var inventoryItem = new InventoryItem
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Location = request.Location,
                UpdatedBy = userId,
                LastUpdated = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _inventoryRepository.CreateAsync(inventoryItem);
        }
        else
        {
            await _inventoryRepository.UpdateQuantityAsync(request.ProductId, request.Quantity, userId);
            
            // Record history
            var history = new InventoryHistory
            {
                ProductId = request.ProductId,
                PreviousQuantity = previousQuantity,
                NewQuantity = request.Quantity,
                ChangeType = ChangeType.Adjustment,
                ChangeAmount = request.Quantity - previousQuantity,
                Location = request.Location,
                ChangedBy = userId,
                ChangedAt = DateTime.UtcNow
            };
            await _historyRepository.CreateAsync(history);
        }

        var updated = await _inventoryRepository.GetByProductIdAsync(request.ProductId);
        return MapToDto(updated!);
    }

    public async Task<InventoryDto?> GetInventoryByProductIdAsync(int productId)
    {
        var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
        return inventory == null ? null : MapToDto(inventory);
    }

    public async Task<IEnumerable<InventoryDto>> GetAllInventoryAsync()
    {
        var inventory = await _inventoryRepository.GetAllAsync();
        return inventory.Select(MapToDto);
    }

    public async Task<IEnumerable<InventoryDto>> GetInventoryByLocationAsync(string location)
    {
        var inventory = await _inventoryRepository.GetByLocationAsync(location);
        return inventory.Select(MapToDto);
    }

    public async Task<InventoryDto> AdjustInventoryAsync(InventoryAdjustmentRequest request, int? userId)
    {
        var existing = await _inventoryRepository.GetByProductIdAsync(request.ProductId);
        var previousQuantity = existing?.Quantity ?? 0;
        var newQuantity = previousQuantity + request.ChangeAmount;

        if (newQuantity < 0)
        {
            throw new Exception("Insufficient inventory");
        }

        var changeType = Enum.Parse<ChangeType>(request.ChangeType);

        if (existing == null)
        {
            var inventoryItem = new InventoryItem
            {
                ProductId = request.ProductId,
                Quantity = newQuantity,
                Location = request.Location,
                UpdatedBy = userId,
                LastUpdated = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _inventoryRepository.CreateAsync(inventoryItem);
        }
        else
        {
            await _inventoryRepository.UpdateQuantityAsync(request.ProductId, newQuantity, userId);
        }

        // Record history
        var history = new InventoryHistory
        {
            ProductId = request.ProductId,
            PreviousQuantity = previousQuantity,
            NewQuantity = newQuantity,
            ChangeType = changeType,
            ChangeAmount = request.ChangeAmount,
            Location = request.Location,
            ChangedBy = userId,
            ChangedAt = DateTime.UtcNow
        };
        await _historyRepository.CreateAsync(history);

        var updated = await _inventoryRepository.GetByProductIdAsync(request.ProductId);
        return MapToDto(updated!);
    }

    public async Task<IEnumerable<InventoryDto>> GetLowStockItemsAsync()
    {
        var lowStockItems = await _inventoryRepository.GetLowStockItemsAsync(0);
        return lowStockItems.Select(MapToDto);
    }

    private static InventoryDto MapToDto(InventoryItem inventory)
    {
        return new InventoryDto
        {
            Id = inventory.Id,
            ProductId = inventory.ProductId,
            ProductName = inventory.Product?.Name,
            ProductSKU = inventory.Product?.SKU,
            Quantity = inventory.Quantity,
            Location = inventory.Location,
            LastUpdated = inventory.LastUpdated
        };
    }
}

