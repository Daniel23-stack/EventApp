namespace EvenApp.Application.DTOs;

public class InventoryDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductSKU { get; set; }
    public int Quantity { get; set; }
    public string? Location { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class UpdateInventoryRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Location { get; set; }
}

public class InventoryAdjustmentRequest
{
    public int ProductId { get; set; }
    public int ChangeAmount { get; set; }
    public string ChangeType { get; set; } = string.Empty; // Purchase, Sale, Adjustment, Return
    public string? Location { get; set; }
}

