namespace EvenApp.Domain.Entities;

public enum ChangeType
{
    Purchase,
    Sale,
    Adjustment,
    Return
}

public class InventoryHistory : BaseEntity
{
    public int ProductId { get; set; }
    public int PreviousQuantity { get; set; }
    public int NewQuantity { get; set; }
    public ChangeType ChangeType { get; set; }
    public int ChangeAmount { get; set; }
    public string? Location { get; set; }
    public int? ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Product? Product { get; set; }
}

