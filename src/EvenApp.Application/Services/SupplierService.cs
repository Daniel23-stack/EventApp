using EvenApp.Application.DTOs;
using EvenApp.Domain.Entities;
using EvenApp.Domain.Interfaces;

namespace EvenApp.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<SupplierDto> CreateSupplierAsync(CreateSupplierRequest request)
    {
        var supplier = new Supplier
        {
            Name = request.Name,
            ContactName = request.ContactName,
            Email = request.Email,
            Phone = request.Phone,
            Address = request.Address,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var id = await _supplierRepository.CreateAsync(supplier);
        var created = await _supplierRepository.GetByIdAsync(id);

        return MapToDto(created!);
    }

    public async Task<SupplierDto?> GetSupplierByIdAsync(int id)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        return supplier == null ? null : MapToDto(supplier);
    }

    public async Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierDto>> SearchSuppliersAsync(string searchTerm)
    {
        var suppliers = await _supplierRepository.SearchAsync(searchTerm);
        return suppliers.Select(MapToDto);
    }

    public async Task<SupplierDto> UpdateSupplierAsync(int id, UpdateSupplierRequest request)
    {
        var supplier = await _supplierRepository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new Exception("Supplier not found");
        }

        supplier.Name = request.Name;
        supplier.ContactName = request.ContactName;
        supplier.Email = request.Email;
        supplier.Phone = request.Phone;
        supplier.Address = request.Address;
        supplier.UpdatedAt = DateTime.UtcNow;

        await _supplierRepository.UpdateAsync(supplier);
        var updated = await _supplierRepository.GetByIdAsync(id);

        return MapToDto(updated!);
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        return await _supplierRepository.DeleteAsync(id);
    }

    private static SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactName = supplier.ContactName,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            CreatedAt = supplier.CreatedAt,
            UpdatedAt = supplier.UpdatedAt
        };
    }
}

