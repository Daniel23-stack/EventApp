using EvenApp.Application.DTOs;

namespace EvenApp.Application.Services;

public interface IAnalyticsService
{
    Task<IEnumerable<SalesForecastDto>> GetSalesForecastAsync(int productId, int days);
    Task<IEnumerable<TurnoverRateDto>> GetTurnoverRatesAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<ABCAnalysisDto>> GetABCAnalysisAsync();
    Task<IEnumerable<InventoryTrendDto>> GetInventoryTrendsAsync(int productId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<ProductPerformanceDto>> GetProductPerformanceAsync(DateTime startDate, DateTime endDate);
}

