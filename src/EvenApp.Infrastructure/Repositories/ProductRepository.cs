using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    protected override string TableName => "Products";

    public ProductRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<Product?> GetBySKUAsync(string sku)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Products WHERE SKU = @SKU";
        return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { SKU = sku });
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT * FROM Products 
                   WHERE Name LIKE @SearchTerm 
                   OR SKU LIKE @SearchTerm 
                   OR Description LIKE @SearchTerm";
        var searchPattern = $"%{searchTerm}%";
        return await connection.QueryAsync<Product>(sql, new { SearchTerm = searchPattern });
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Products WHERE Category = @Category";
        return await connection.QueryAsync<Product>(sql, new { Category = category });
    }

    public async Task<bool> SKUExistsAsync(string sku)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Products WHERE SKU = @SKU";
        var count = await connection.QuerySingleAsync<int>(sql, new { SKU = sku });
        return count > 0;
    }
}

