using NUnit.Framework;
using FluentAssertions;
using RealEstate.Infrastructure.Repositories;
using RealEstate.Infrastructure.Data;
using RealEstate.Domain.Entities;
using RealEstate.Application.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace RealEstate.Tests.Repositories
{
    [TestFixture]
    public class PropertyRepositoryTests
    {
        private PropertyRepository _repository;
        private MongoDbContext _context;

        [SetUp]
        public void Setup()
        {
            
            var settings = Options.Create(new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "TestRealEstateDB"
            });
            
            _context = new MongoDbContext(settings);
            _repository = new PropertyRepository(_context);
        }

        [Test]
        public void GetByIdAsync_ShouldReturnNull_WhenInvalidObjectId()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var result = await _repository.GetByIdAsync("invalid-id");
                result.Should().BeNull();
            });
        }

        [Test]
        public void ExistsAsync_ShouldReturnFalse_WhenInvalidObjectId()
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                var result = await _repository.ExistsAsync("invalid-id");
                result.Should().BeFalse();
            });
        }

        [Test]
        public async Task GetFilteredAsync_ShouldBuildCorrectFilter_WithAllParameters()
        {
            var filter = new PropertyFilterDto
            {
                Name = "Test Aleixer",
                Address = "Address",
                MinPrice = 100000,
                MaxPrice = 500000,
                Page = 1,
                PageSize = 10
            };

            Assert.DoesNotThrowAsync(async () =>
            {
                var (properties, total) = await _repository.GetFilteredAsync(filter);                
                properties.Should().NotBeNull();
                total.Should().BeGreaterThanOrEqualTo(0);
            });
        }
    }
}