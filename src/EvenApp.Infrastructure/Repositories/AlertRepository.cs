using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    protected override string TableName => "Alerts";

    public AlertRepository(IDbConnectionFactory connectionFactory) : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<Alert>> GetActiveAlertsAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Alerts WHERE IsResolved = FALSE ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<Alert>(sql);
    }

    public async Task<IEnumerable<Alert>> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Alerts WHERE ProductId = @ProductId ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<Alert>(sql, new { ProductId = productId });
    }

    public async Task<IEnumerable<Alert>> GetByTypeAsync(AlertType type)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Alerts WHERE Type = @Type ORDER BY CreatedAt DESC";
        return await connection.QueryAsync<Alert>(sql, new { Type = type.ToString() });
    }

    public async Task<bool> ResolveAlertAsync(int alertId, int userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"UPDATE Alerts 
                   SET IsResolved = TRUE, ResolvedAt = NOW(), ResolvedBy = @UserId 
                   WHERE Id = @AlertId";
        var rowsAffected = await connection.ExecuteAsync(sql, new { AlertId = alertId, UserId = userId });
        return rowsAffected > 0;
    }
}

