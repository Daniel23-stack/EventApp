using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<int> CreateAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetByProductIdAsync(int productId);
    Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetByTransactionTypeAsync(TransactionType type);
}

