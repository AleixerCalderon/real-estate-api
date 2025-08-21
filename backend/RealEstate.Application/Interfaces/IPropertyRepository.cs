using RealEstate.Domain.Entities;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Property?> GetByIdAsync(string id);
        Task<(IEnumerable<Property> Properties, int Total)> GetFilteredAsync(PropertyFilterDto filter);
        Task<Property> CreateAsync(Property property);
        Task<Property?> UpdateAsync(string id, Property property);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}