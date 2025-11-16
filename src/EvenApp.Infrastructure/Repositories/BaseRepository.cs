using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly IDbConnectionFactory _connectionFactory;
    protected abstract string TableName { get; }

    protected BaseRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        connection.Open();
        var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";
        return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = $"SELECT * FROM {TableName}";
        return await connection.QueryAsync<T>(sql);
    }

    public virtual async Task<int> CreateAsync(T entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "CreatedAt" && p.Name != "UpdatedAt")
            .Select(p => p.Name);
        
        var columns = string.Join(", ", properties);
        var values = string.Join(", ", properties.Select(p => $"@{p}"));
        var sql = $"INSERT INTO {TableName} ({columns}) VALUES ({values}); SELECT LAST_INSERT_ID();";
        
        return await connection.QuerySingleAsync<int>(sql, entity);
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        using var connection = _connectionFactory.CreateConnection();
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name != "Id" && p.Name != "CreatedAt")
            .Select(p => p.Name);
        
        var setClause = string.Join(", ", properties.Select(p => $"{p} = @{p}"));
        var sql = $"UPDATE {TableName} SET {setClause} WHERE Id = @Id";
        
        var rowsAffected = await connection.ExecuteAsync(sql, entity);
        return rowsAffected > 0;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = $"DELETE FROM {TableName} WHERE Id = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        return rowsAffected > 0;
    }
}

