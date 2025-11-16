using EvenApp.Application.DTOs;

namespace EvenApp.Application.Services;

public interface IAlertService
{
    Task<AlertDto> CreateAlertAsync(CreateAlertRequest request);
    Task<AlertDto?> GetAlertByIdAsync(int id);
    Task<IEnumerable<AlertDto>> GetActiveAlertsAsync();
    Task<IEnumerable<AlertDto>> GetAlertsByProductIdAsync(int productId);
    Task<bool> ResolveAlertAsync(int alertId, int userId);
    Task CheckReorderLevelsAsync();
}

