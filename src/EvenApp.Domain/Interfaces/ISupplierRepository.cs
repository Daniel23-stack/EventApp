using EvenApp.Domain.Entities;

namespace EvenApp.Domain.Interfaces;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<IEnumerable<Supplier>> SearchAsync(string searchTerm);
}

