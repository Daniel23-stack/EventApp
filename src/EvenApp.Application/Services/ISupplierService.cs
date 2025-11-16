using EvenApp.Application.DTOs;

namespace EvenApp.Application.Services;

public interface ISupplierService
{
    Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request);
    Task<SupplierDto?> GetSupplierByIdAsync(int id);
    Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
    Task<IEnumerable<SupplierDto>> SearchSuppliersAsync(string searchTerm);
    Task<SupplierDto> UpdateSupplierAsync(int id, UpdateSupplierRequest request);
    Task<bool> DeleteSupplierAsync(int id);
}

