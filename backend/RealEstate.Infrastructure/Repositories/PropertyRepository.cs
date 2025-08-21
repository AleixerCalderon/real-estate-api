using MongoDB.Driver;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using RealEstate.Application.DTOs;
using MongoDB.Bson;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly MongoDbContext _context;

        public PropertyRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties.Find(_ => true).ToListAsync();
        }

        public async Task<Property?> GetByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return null;

            return await _context.Properties.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Property> Properties, int Total)> GetFilteredAsync(PropertyFilterDto filter)
        {
            var filterBuilder = Builders<Property>.Filter;
            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                filters.Add(filterBuilder.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));
            }

            if (!string.IsNullOrEmpty(filter.Address))
            {
                filters.Add(filterBuilder.Regex(p => p.Address, new BsonRegularExpression(filter.Address, "i")));
            }

            if (filter.MinPrice.HasValue)
            {
                filters.Add(filterBuilder.Gte(p => p.Price, filter.MinPrice.Value));
            }

            if (filter.MaxPrice.HasValue)
            {
                filters.Add(filterBuilder.Lte(p => p.Price, filter.MaxPrice.Value));
            }

            var combinedFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

            var total = await _context.Properties.CountDocumentsAsync(combinedFilter);
            var properties = await _context.Properties
                .Find(combinedFilter)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Limit(filter.PageSize)
                .ToListAsync();

            return (properties, (int)total);
        }

        public async Task<Property> CreateAsync(Property property)
        {
            property.Id = ObjectId.GenerateNewId().ToString();
            await _context.Properties.InsertOneAsync(property);
            return property;
        }

        public async Task<Property?> UpdateAsync(string id, Property property)
        {
            if (!ObjectId.TryParse(id, out _))
                return null;

            property.Id = id;
            var result = await _context.Properties.ReplaceOneAsync(p => p.Id == id, property);
            return result.ModifiedCount > 0 ? property : null;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return false;

            var result = await _context.Properties.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return false;

            var count = await _context.Properties.CountDocumentsAsync(p => p.Id == id);
            return count > 0;
        }
    }
}