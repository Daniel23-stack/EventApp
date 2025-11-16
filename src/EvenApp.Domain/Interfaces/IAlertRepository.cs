using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface IAlertRepository : IRepository<Alert>
{
    Task<IEnumerable<Alert>> GetActiveAlertsAsync();
    Task<IEnumerable<Alert>> GetByProductIdAsync(int productId);
    Task<IEnumerable<Alert>> GetByTypeAsync(AlertType type);
    Task<bool> ResolveAlertAsync(int alertId, int userId);
}

