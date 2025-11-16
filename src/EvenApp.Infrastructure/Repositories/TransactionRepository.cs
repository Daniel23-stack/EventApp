using Dapper;
using EvenApp.Application.Interfaces;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;
using System.Data;

namespace EvenApp.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public TransactionRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(Transaction transaction)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = @"INSERT INTO Transactions 
                   (ProductId, TransactionType, Quantity, UnitPrice, TotalAmount, TransactionDate, CreatedBy) 
                   VALUES 
                   (@ProductId, @TransactionType, @Quantity, @UnitPrice, @TotalAmount, @TransactionDate, @CreatedBy);
                   SELECT LAST_INSERT_ID();";
        
        return await connection.QuerySingleAsync<int>(sql, transaction);
    }

    public async Task<IEnumerable<Transaction>> GetByProductIdAsync(int productId)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Transactions WHERE ProductId = @ProductId ORDER BY TransactionDate DESC";
        return await connection.QueryAsync<Transaction>(sql, new { ProductId = productId });
    }

    public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Transactions WHERE TransactionDate BETWEEN @StartDate AND @EndDate ORDER BY TransactionDate DESC";
        return await connection.QueryAsync<Transaction>(sql, new { StartDate = startDate, EndDate = endDate });
    }

    public async Task<IEnumerable<Transaction>> GetByTransactionTypeAsync(TransactionType type)
    {
        using var connection = _connectionFactory.CreateConnection();
        var sql = "SELECT * FROM Transactions WHERE TransactionType = @TransactionType ORDER BY TransactionDate DESC";
        return await connection.QueryAsync<Transaction>(sql, new { TransactionType = type.ToString() });
    }
}

