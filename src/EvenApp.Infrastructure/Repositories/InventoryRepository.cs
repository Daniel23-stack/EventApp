using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class InventoryRepository : BaseRepository<InventoryItem>, IInventoryRepository
{
    protected override string TableName => "Inventory";

    public InventoryRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<InventoryItem?> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        var sql = @"SELECT i.*, p.Id as Product_Id, p.SKU as Product_SKU, p.Name as Product_Name, 
                   p.Description as Product_Description, p.Category as Product_Category, 
                   p.UnitPrice as Product_UnitPrice, p.ReorderLevel as Product_ReorderLevel,
                   p.CreatedAt as Product_CreatedAt, p.UpdatedAt as Product_UpdatedAt
                   FROM Inventory i 
                   INNER JOIN Products p ON i.ProductId = p.Id 
                   WHERE i.ProductId = @ProductId";
        
        var result = await connection.QueryAsync(sql, new { ProductId = productId });
        var first = result.FirstOrDefault();
        if (first == null) return null;
        
        var inventory = new InventoryItem
        {
            Id = (int)first.Id,
            ProductId = (int)first.ProductId,
            Quantity = (int)first.Quantity,
            Location = first.Location?.ToString(),
            UpdatedBy = first.UpdatedBy != null ? (int?)first.UpdatedBy : null,
            LastUpdated = (DateTime)first.LastUpdated,
            CreatedAt = (DateTime)first.CreatedAt,
            UpdatedAt = (DateTime)first.UpdatedAt,
            Product = new Product
            {
                Id = (int)first.Product_Id,
                SKU = first.Product_SKU?.ToString() ?? string.Empty,
                Name = first.Product_Name?.ToString() ?? string.Empty,
                Description = first.Product_Description?.ToString(),
                Category = first.Product_Category?.ToString(),
                UnitPrice = (decimal)first.Product_UnitPrice,
                ReorderLevel = (int)first.Product_ReorderLevel,
                CreatedAt = (DateTime)first.Product_CreatedAt,
                UpdatedAt = (DateTime)first.Product_UpdatedAt
            }
        };
        
        return inventory;
    }

    public async Task<IEnumerable<InventoryItem>> GetByLocationAsync(string location)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Inventory WHERE Location = @Location";
        return await connection.QueryAsync<InventoryItem>(sql, new { Location = location });
    }

    public async Task<bool> UpdateQuantityAsync(int productId, int quantity, int? userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"UPDATE Inventory 
                   SET Quantity = @Quantity, UpdatedBy = @UpdatedBy, LastUpdated = NOW() 
                   WHERE ProductId = @ProductId";
        
        var rowsAffected = await connection.ExecuteAsync(sql, new 
        { 
            ProductId = productId, 
            Quantity = quantity, 
            UpdatedBy = userId 
        });
        
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync(int threshold)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        var sql = @"SELECT i.*, p.Id as Product_Id, p.SKU as Product_SKU, p.Name as Product_Name
                   FROM Inventory i 
                   INNER JOIN Products p ON i.ProductId = p.Id 
                   WHERE i.Quantity <= @Threshold";
        
        var results = await connection.QueryAsync(sql, new { Threshold = threshold });
        var items = new List<InventoryItem>();
        
        foreach (var row in results)
        {
            items.Add(new InventoryItem
            {
                Id = (int)row.Id,
                ProductId = (int)row.ProductId,
                Quantity = (int)row.Quantity,
                Location = row.Location?.ToString(),
                UpdatedBy = row.UpdatedBy != null ? (int?)row.UpdatedBy : null,
                LastUpdated = (DateTime)row.LastUpdated,
                CreatedAt = (DateTime)row.CreatedAt,
                UpdatedAt = (DateTime)row.UpdatedAt,
                Product = new Product
                {
                    Id = (int)row.Product_Id,
                    SKU = row.Product_SKU?.ToString() ?? string.Empty,
                    Name = row.Product_Name?.ToString() ?? string.Empty
                }
            });
        }
        
        return items;
    }
}

