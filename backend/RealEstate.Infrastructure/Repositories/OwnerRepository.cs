using MongoDB.Driver;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using MongoDB.Bson;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly MongoDbContext _context;

        public OwnerRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await _context.Owners.Find(_ => true)
                .SortBy(o => o.Name)
                .ToListAsync();
        }

        public async Task<Owner?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return null;

            return await _context.Owners.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Owner> CreateAsync(Owner owner)
        {
            await _context.Owners.InsertOneAsync(owner);
            return owner;
        }

        public async Task<Owner?> UpdateAsync(string id, Owner owner)
        {
            if (!ObjectId.TryParse(id, out _))
                return null;

            owner.Id = id;
            var result = await _context.Owners.ReplaceOneAsync(o => o.Id == id, owner);
            return result.ModifiedCount > 0 ? owner : null;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return false;

            // Verificar que no haya propiedades asociadas al propietario AAB (20 08 2025)
            var propertiesCount = await _context.Properties.CountDocumentsAsync(p => p.IdOwner == id);
            if (propertiesCount > 0)
            {
                throw new InvalidOperationException("No se puede eliminar el propietario porque tiene propiedades asociadas");
            }

            var result = await _context.Owners.DeleteOneAsync(o => o.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return false;

            var count = await _context.Owners.CountDocumentsAsync(o => o.Id == id);
            return count > 0;
        }
    }
}