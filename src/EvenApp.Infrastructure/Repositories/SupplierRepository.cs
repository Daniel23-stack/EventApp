using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
{
    protected override string TableName => "Suppliers";

    public SupplierRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<Supplier>> SearchAsync(string searchTerm)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"SELECT * FROM Suppliers 
                   WHERE Name LIKE @SearchTerm 
                   OR ContactName LIKE @SearchTerm 
                   OR Email LIKE @SearchTerm";
        var searchPattern = $"%{searchTerm}%";
        return await connection.QueryAsync<Supplier>(sql, new { SearchTerm = searchPattern });
    }
}

