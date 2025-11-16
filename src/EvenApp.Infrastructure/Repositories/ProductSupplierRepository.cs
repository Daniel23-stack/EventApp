using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class ProductSupplierRepository : IProductSupplierRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public ProductSupplierRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(ProductSupplier productSupplier)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO ProductSuppliers 
                   (ProductId, SupplierId, SupplierSKU, UnitCost) 
                   VALUES 
                   (@ProductId, @SupplierId, @SupplierSKU, @UnitCost);
                   SELECT LAST_INSERT_ID();";
        
        return await connection.QuerySingleAsync<int>(sql, productSupplier);
    }

    public async Task<IEnumerable<ProductSupplier>> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT ps.*, p.*, s.* 
                   FROM ProductSuppliers ps 
                   INNER JOIN Products p ON ps.ProductId = p.Id 
                   INNER JOIN Suppliers s ON ps.SupplierId = s.Id 
                   WHERE ps.ProductId = @ProductId";
        
        return await connection.QueryAsync<ProductSupplier, Product, Supplier, ProductSupplier>(
            sql,
            (ps, product, supplier) =>
            {
                ps.Product = product;
                ps.Supplier = supplier;
                return ps;
            },
            new { ProductId = productId },
            splitOn: "Id");
    }

    public async Task<IEnumerable<ProductSupplier>> GetBySupplierIdAsync(int supplierId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT ps.*, p.*, s.* 
                   FROM ProductSuppliers ps 
                   INNER JOIN Products p ON ps.ProductId = p.Id 
                   INNER JOIN Suppliers s ON ps.SupplierId = s.Id 
                   WHERE ps.SupplierId = @SupplierId";
        
        return await connection.QueryAsync<ProductSupplier, Product, Supplier, ProductSupplier>(
            sql,
            (ps, product, supplier) =>
            {
                ps.Product = product;
                ps.Supplier = supplier;
                return ps;
            },
            new { SupplierId = supplierId },
            splitOn: "Id");
    }

    public async Task<bool> DeleteAsync(int productId, int supplierId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "DELETE FROM ProductSuppliers WHERE ProductId = @ProductId AND SupplierId = @SupplierId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { ProductId = productId, SupplierId = supplierId });
        return rowsAffected > 0;
    }
}

