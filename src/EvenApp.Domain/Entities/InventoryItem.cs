namespace EvenApp.Domain.Entities;

public class InventoryItem : BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Location { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Product? Product { get; set; }
}

