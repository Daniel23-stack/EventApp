namespace EvenApp.Domain.Entities;

public enum TransactionType
{
    Sale,
    Purchase,
    Return,
    Adjustment
}

public class Transaction : BaseEntity
{
    public int ProductId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }
    
    // Navigation property
    public Product? Product { get; set; }
}

