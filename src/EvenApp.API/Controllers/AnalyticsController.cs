using EvenApp.Application.DTOs;
using EvenApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvenApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("sales-forecast/{productId}")]
    public async Task<ActionResult<IEnumerable<SalesForecastDto>>> GetSalesForecast(
        int productId, 
        [FromQuery] int days = 30)
    {
        var forecast = await _analyticsService.GetSalesForecastAsync(productId, days);
        return Ok(forecast);
    }

    [HttpGet("turnover-rates")]
    public async Task<ActionResult<IEnumerable<TurnoverRateDto>>> GetTurnoverRates(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;
        
        var turnoverRates = await _analyticsService.GetTurnoverRatesAsync(start, end);
        return Ok(turnoverRates);
    }

    [HttpGet("abc-analysis")]
    public async Task<ActionResult<IEnumerable<ABCAnalysisDto>>> GetABCAnalysis()
    {
        var analysis = await _analyticsService.GetABCAnalysisAsync();
        return Ok(analysis);
    }

    [HttpGet("inventory-trends/{productId}")]
    public async Task<ActionResult<IEnumerable<InventoryTrendDto>>> GetInventoryTrends(
        int productId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;
        
        var trends = await _analyticsService.GetInventoryTrendsAsync(productId, start, end);
        return Ok(trends);
    }

    [HttpGet("product-performance")]
    public async Task<ActionResult<IEnumerable<ProductPerformanceDto>>> GetProductPerformance(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var start = startDate ?? DateTime.UtcNow.AddDays(-30);
        var end = endDate ?? DateTime.UtcNow;
        
        var performance = await _analyticsService.GetProductPerformanceAsync(start, end);
        return Ok(performance);
    }
}

