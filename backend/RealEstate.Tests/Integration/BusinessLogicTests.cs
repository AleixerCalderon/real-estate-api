using NUnit.Framework;
using Moq;
using FluentAssertions;
using RealEstate.Application.Services;
using RealEstate.Application.Interfaces;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Integration
{
    [TestFixture]
    public class BusinessLogicTests
    {
        private Mock<IPropertyRepository> _mockPropertyRepository;
        private Mock<IOwnerRepository> _mockOwnerRepository;
        private PropertyService _propertyService;

        [SetUp]
        public void Setup()
        {
            _mockPropertyRepository = new Mock<IPropertyRepository>();
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _propertyService = new PropertyService(_mockPropertyRepository.Object, _mockOwnerRepository.Object);
        }

        [Test]
        public async Task CreateProperty_ShouldGenerateCodeInternal_WhenPropertyIsCreated()
        {
            
            var propertyDto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                IdOwner = "valid-owner",
                Year = 2023
            };

            var owner = new Owner { Id = "valid-owner", Name = "Test Owner" };

            _mockOwnerRepository.Setup(x => x.ExistsAsync("valid-owner")).ReturnsAsync(true);
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("valid-owner")).ReturnsAsync(owner);

            Property capturedProperty = null!;
            _mockPropertyRepository.Setup(x => x.CreateAsync(It.IsAny<Property>()))
                .Callback<Property>(p => capturedProperty = p)
                .ReturnsAsync((Property p) => p);

            
            var result = await _propertyService.CreatePropertyAsync(propertyDto);

            
            result.Success.Should().BeTrue();
            capturedProperty.Should().NotBeNull();
            capturedProperty.CodeInternal.Should().BeGreaterThan(0);
            capturedProperty.CodeInternal.Should().BeInRange(100000, 999999);
        }

        [Test]
        public async Task UpdateProperty_ShouldOnlyUpdateProvidedFields()
        {
            
            var existingProperty = new Property
            {
                Id = "1",
                Name = "Original Name",
                Address = "Original Address",
                Price = 100000,
                Year = 2020
            };

            var updateDto = new PropertyUpdateDto
            {
                Name = "Updated Name",
                Price = 150000
                
            };

            var owner = new Owner { Id = "owner1", Name = "Test Owner" };

            _mockPropertyRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(existingProperty);
            _mockOwnerRepository.Setup(x => x.GetByIdAsync(existingProperty.IdOwner)).ReturnsAsync(owner);

            Property capturedProperty = null!;
            _mockPropertyRepository.Setup(x => x.UpdateAsync("1", It.IsAny<Property>()))
                .Callback<string, Property>((id, p) => capturedProperty = p)
                .ReturnsAsync((string id, Property p) => p);

            
            var result = await _propertyService.UpdatePropertyAsync("1", updateDto);

            
            result.Success.Should().BeTrue();
            capturedProperty.Should().NotBeNull();
            capturedProperty.Name.Should().Be("Updated Name");
            capturedProperty.Price.Should().Be(150000);
            capturedProperty.Address.Should().Be("Original Address"); // No cambia AAB (20 08 2025)
            capturedProperty.Year.Should().Be(2020); // No cambia
        }

        [Test]
        public async Task FilterProperties_ShouldApplyAllFiltersCorrectly()
        {
            
            var filter = new PropertyFilterDto
            {
                Name = "Test",
                Address = "Address",
                MinPrice = 50000,
                MaxPrice = 150000,
                Page = 2,
                PageSize = 5
            };

            var properties = new List<Property>
            {
                new Property { Name = "Test Property", Price = 100000, IdOwner = "owner1" }
            };
            var owner = new Owner { Id = "owner1", Name = "Test Owner" };

            _mockPropertyRepository.Setup(x => x.GetFilteredAsync(filter))
                .ReturnsAsync((properties, 10));
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("owner1")).ReturnsAsync(owner);

            
            var result = await _propertyService.GetFilteredPropertiesAsync(filter);

            
            result.Success.Should().BeTrue();
            result.Total.Should().Be(10);
            result.Page.Should().Be(2);
            result.PageSize.Should().Be(5);
            _mockPropertyRepository.Verify(x => x.GetFilteredAsync(
                It.Is<PropertyFilterDto>(f => 
                    f.Name == "Test" && 
                    f.Address == "Address" && 
                    f.MinPrice == 50000 && 
                    f.MaxPrice == 150000 &&
                    f.Page == 2 &&
                    f.PageSize == 5)), Times.Once);
        }
    }
}