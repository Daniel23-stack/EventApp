namespace EvenApp.Application.DTOs;

public class SalesForecastDto
{
    public DateTime Date { get; set; }
    public decimal ForecastedSales { get; set; }
    public decimal Confidence { get; set; }
}

public class TurnoverRateDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public decimal TurnoverRate { get; set; }
    public int AverageInventory { get; set; }
    public int TotalSales { get; set; }
    public int PeriodDays { get; set; }
}

public class ABCAnalysisDto
{
    public string Category { get; set; } = string.Empty; // A, B, or C
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public decimal PercentageOfTotal { get; set; }
    public decimal CumulativePercentage { get; set; }
}

public class InventoryTrendDto
{
    public DateTime Date { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Value { get; set; }
}

public class ProductPerformanceDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductSKU { get; set; } = string.Empty;
    public int TotalSales { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AveragePrice { get; set; }
    public int UnitsInStock { get; set; }
    public decimal StockValue { get; set; }
}

