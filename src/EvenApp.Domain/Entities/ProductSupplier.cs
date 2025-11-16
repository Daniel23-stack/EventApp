namespace EvenApp.Domain.Entities;

public class ProductSupplier : BaseEntity
{
    public int ProductId { get; set; }
    public int SupplierId { get; set; }
    public string? SupplierSKU { get; set; }
    public decimal? UnitCost { get; set; }
    
    // Navigation properties
    public Product? Product { get; set; }
    public Supplier? Supplier { get; set; }
}

