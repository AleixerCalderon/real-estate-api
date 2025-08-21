using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerRepository
    {
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<Owner?> GetByIdAsync(string id);
        Task<Owner> CreateAsync(Owner owner);
        Task<Owner?> UpdateAsync(string id, Owner owner);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}