namespace EvenApp.Domain.Entities;

public enum AlertType
{
    LowStock,
    Reorder,
    Expiring,
    Custom
}

public class Alert : BaseEntity
{
    public AlertType Type { get; set; }
    public int? ProductId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsResolved { get; set; } = false;
    public DateTime? ResolvedAt { get; set; }
    public int? ResolvedBy { get; set; }
    
    // Navigation property
    public Product? Product { get; set; }
}

