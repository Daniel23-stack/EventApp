using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    protected override string TableName => "Orders";

    public OrderRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public override async Task<Order?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var orderSql = "SELECT * FROM Orders WHERE Id = @Id";
        var order = await connection.QueryFirstOrDefaultAsync<Order>(orderSql, new { Id = id });
        
        if (order == null) return null;
        
        var itemsSql = @"SELECT oi.*, p.* 
                        FROM OrderItems oi 
                        INNER JOIN Products p ON oi.ProductId = p.Id 
                        WHERE oi.OrderId = @OrderId";
        
        var items = await connection.QueryAsync<OrderItem, Product, OrderItem>(
            itemsSql,
            (item, product) =>
            {
                item.Product = product;
                return item;
            },
            new { OrderId = id },
            splitOn: "Id");
        
        order.Items = items.ToList();
        
        var supplierSql = "SELECT * FROM Suppliers WHERE Id = @SupplierId";
        order.Supplier = await connection.QueryFirstOrDefaultAsync<Supplier>(supplierSql, new { SupplierId = order.SupplierId });
        
        return order;
    }

    public async Task<IEnumerable<Order>> GetBySupplierIdAsync(int supplierId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Orders WHERE SupplierId = @SupplierId ORDER BY OrderDate DESC";
        return await connection.QueryAsync<Order>(sql, new { SupplierId = supplierId });
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Orders WHERE Status = @Status ORDER BY OrderDate DESC";
        return await connection.QueryAsync<Order>(sql, new { Status = status.ToString() });
    }

    public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Orders WHERE OrderDate BETWEEN @StartDate AND @EndDate ORDER BY OrderDate DESC";
        return await connection.QueryAsync<Order>(sql, new { StartDate = startDate, EndDate = endDate });
    }

    public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus status)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "UPDATE Orders SET Status = @Status, UpdatedAt = NOW() WHERE Id = @OrderId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { OrderId = orderId, Status = status.ToString() });
        return rowsAffected > 0;
    }
}

