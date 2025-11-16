using EvenApp.Application.DTOs;

namespace EvenApp.Application.Services;

public interface IInventoryService
{
    Task<InventoryDto> UpdateInventoryAsync(UpdateInventoryRequest request, int? userId);
    Task<InventoryDto?> GetInventoryByProductIdAsync(int productId);
    Task<IEnumerable<InventoryDto>> GetAllInventoryAsync();
    Task<IEnumerable<InventoryDto>> GetInventoryByLocationAsync(string location);
    Task<InventoryDto> AdjustInventoryAsync(InventoryAdjustmentRequest request, int? userId);
    Task<IEnumerable<InventoryDto>> GetLowStockItemsAsync();
}

