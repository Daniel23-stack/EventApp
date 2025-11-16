using EvenApp.Application.DTOs;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IProductRepository _productRepository;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AnalyticsService(
        IProductRepository productRepository,
        IInventoryRepository inventoryRepository,
        ITransactionRepository transactionRepository)
    {
        _productRepository = productRepository;
        _inventoryRepository = inventoryRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<SalesForecastDto>> GetSalesForecastAsync(int productId, int days)
    {
        // Get historical sales data
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddDays(-90); // Use last 90 days for forecasting
        
        var transactions = await _transactionRepository.GetByProductIdAsync(productId);
        var salesTransactions = transactions
            .Where(t => t.TransactionType == TransactionType.Sale && 
                       t.TransactionDate >= startDate && 
                       t.TransactionDate <= endDate)
            .OrderBy(t => t.TransactionDate)
            .ToList();

        if (!salesTransactions.Any())
        {
            return new List<SalesForecastDto>();
        }

        // Simple moving average forecast
        var dailySales = salesTransactions
            .GroupBy(t => t.TransactionDate.Date)
            .Select(g => new { Date = g.Key, Sales = g.Sum(t => t.TotalAmount) })
            .ToList();

        var averageDailySales = dailySales.Average(d => d.Sales);
        var forecast = new List<SalesForecastDto>();

        for (int i = 1; i <= days; i++)
        {
            var forecastDate = endDate.AddDays(i);
            forecast.Add(new SalesForecastDto
            {
                Date = forecastDate,
                ForecastedSales = averageDailySales,
                Confidence = 0.75m // Simple confidence score
            });
        }

        return forecast;
    }

    public async Task<IEnumerable<TurnoverRateDto>> GetTurnoverRatesAsync(DateTime startDate, DateTime endDate)
    {
        // Get transactions for the date range
        var transactions = await _transactionRepository.GetByDateRangeAsync(startDate, endDate);
        var salesTransactions = transactions.Where(t => t.TransactionType == TransactionType.Sale).ToList();
        
        var productIds = salesTransactions.Select(t => t.ProductId).Distinct();
        var turnoverRates = new List<TurnoverRateDto>();
        
        foreach (var productId in productIds)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) continue;
            
            var productSales = salesTransactions.Where(t => t.ProductId == productId).ToList();
            var totalSales = productSales.Sum(t => t.Quantity);
            
            var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
            var avgInventory = inventory?.Quantity ?? 0;
            
            var periodDays = (endDate - startDate).Days;
            var turnoverRate = avgInventory > 0 ? (decimal)totalSales / (decimal)avgInventory : 0;
            
            turnoverRates.Add(new TurnoverRateDto
            {
                ProductId = productId,
                ProductName = product.Name,
                ProductSKU = product.SKU,
                TurnoverRate = turnoverRate,
                AverageInventory = avgInventory,
                TotalSales = totalSales,
                PeriodDays = periodDays
            });
        }
        
        return turnoverRates;
    }

    public async Task<IEnumerable<ABCAnalysisDto>> GetABCAnalysisAsync()
    {
        var transactions = await _transactionRepository.GetByTransactionTypeAsync(TransactionType.Sale);
        var productValues = transactions
            .GroupBy(t => t.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                TotalValue = g.Sum(t => t.TotalAmount)
            })
            .OrderByDescending(p => p.TotalValue)
            .ToList();
        
        var totalValue = productValues.Sum(p => p.TotalValue);
        var abcAnalysis = new List<ABCAnalysisDto>();
        decimal cumulativePercentage = 0;

        foreach (var pv in productValues)
        {
            var product = await _productRepository.GetByIdAsync(pv.ProductId);
            if (product == null) continue;
            
            var percentage = totalValue > 0 ? (pv.TotalValue / totalValue) * 100 : 0;
            cumulativePercentage += percentage;

            string category;
            if (cumulativePercentage <= 80)
                category = "A";
            else if (cumulativePercentage <= 95)
                category = "B";
            else
                category = "C";

            abcAnalysis.Add(new ABCAnalysisDto
            {
                Category = category,
                ProductId = pv.ProductId,
                ProductName = product.Name,
                ProductSKU = product.SKU,
                TotalValue = pv.TotalValue,
                PercentageOfTotal = percentage,
                CumulativePercentage = cumulativePercentage
            });
        }

        return abcAnalysis;
    }

    public async Task<IEnumerable<InventoryTrendDto>> GetInventoryTrendsAsync(int productId, DateTime startDate, DateTime endDate)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return new List<InventoryTrendDto>();
        
        // Simplified version - in production, you'd use IInventoryHistoryRepository
        var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
        if (inventory == null) return new List<InventoryTrendDto>();
        
        return new List<InventoryTrendDto>
        {
            new InventoryTrendDto
            {
                Date = DateTime.UtcNow,
                ProductId = productId,
                ProductName = product.Name,
                Quantity = inventory.Quantity,
                Value = inventory.Quantity * product.UnitPrice
            }
        };
    }

    public async Task<IEnumerable<ProductPerformanceDto>> GetProductPerformanceAsync(DateTime startDate, DateTime endDate)
    {
        var products = await _productRepository.GetAllAsync();
        var transactions = await _transactionRepository.GetByDateRangeAsync(startDate, endDate);
        var salesTransactions = transactions.Where(t => t.TransactionType == TransactionType.Sale).ToList();
        
        var performance = new List<ProductPerformanceDto>();
        
        foreach (var product in products)
        {
            var productSales = salesTransactions.Where(t => t.ProductId == product.Id).ToList();
            var totalSales = productSales.Sum(t => t.Quantity);
            var totalRevenue = productSales.Sum(t => t.TotalAmount);
            var averagePrice = productSales.Any() ? productSales.Average(t => t.UnitPrice) : product.UnitPrice;
            
            var inventory = await _inventoryRepository.GetByProductIdAsync(product.Id);
            var unitsInStock = inventory?.Quantity ?? 0;
            var stockValue = unitsInStock * product.UnitPrice;
            
            performance.Add(new ProductPerformanceDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSKU = product.SKU,
                TotalSales = totalSales,
                TotalRevenue = totalRevenue,
                AveragePrice = averagePrice,
                UnitsInStock = unitsInStock,
                StockValue = stockValue
            });
        }
        
        return performance;
    }
}

