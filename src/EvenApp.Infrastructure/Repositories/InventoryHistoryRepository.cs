using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class InventoryHistoryRepository : IInventoryHistoryRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public InventoryHistoryRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(InventoryHistory history)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO InventoryHistory 
                   (ProductId, PreviousQuantity, NewQuantity, ChangeType, ChangeAmount, Location, ChangedBy, ChangedAt) 
                   VALUES 
                   (@ProductId, @PreviousQuantity, @NewQuantity, @ChangeType, @ChangeAmount, @Location, @ChangedBy, @ChangedAt);
                   SELECT LAST_INSERT_ID();";
        
        return await connection.QuerySingleAsync<int>(sql, history);
    }

    public async Task<IEnumerable<InventoryHistory>> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM InventoryHistory WHERE ProductId = @ProductId ORDER BY ChangedAt DESC";
        return await connection.QueryAsync<InventoryHistory>(sql, new { ProductId = productId });
    }

    public async Task<IEnumerable<InventoryHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM InventoryHistory WHERE ChangedAt BETWEEN @StartDate AND @EndDate ORDER BY ChangedAt DESC";
        return await connection.QueryAsync<InventoryHistory>(sql, new { StartDate = startDate, EndDate = endDate });
    }

    public async Task<IEnumerable<InventoryHistory>> GetByChangeTypeAsync(ChangeType changeType)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM InventoryHistory WHERE ChangeType = @ChangeType ORDER BY ChangedAt DESC";
        return await connection.QueryAsync<InventoryHistory>(sql, new { ChangeType = changeType.ToString() });
    }
}

