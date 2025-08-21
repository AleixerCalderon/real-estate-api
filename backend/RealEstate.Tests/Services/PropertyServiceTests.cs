using NUnit.Framework;
using Moq;
using FluentAssertions;
using RealEstate.Application.Services;
using RealEstate.Application.Interfaces;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Services
{
    [TestFixture]
    public class PropertyServiceTests
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
        public async Task GetAllPropertiesAsync_ShouldReturnSuccess_WhenPropertiesExist()
        {
            
            var properties = new List<Property>
            {
                new Property 
                { 
                    Id = "1", 
                    Name = "Test Property", 
                    IdOwner = "owner1",
                    Address = "Test Address",
                    Price = 100000,
                    Year = 2020,
                    CodeInternal = 12345
                }
            };
            var owner = new Owner { Id = "owner1", Name = "Test Owner" };

            _mockPropertyRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(properties);
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("owner1")).ReturnsAsync(owner);

            
            var result = await _propertyService.GetAllPropertiesAsync();

            
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(1);
            result.Data!.First().Name.Should().Be("Test Property");
            result.Data!.First().OwnerName.Should().Be("Test Owner");
        }

        [Test]
        public async Task GetPropertyByIdAsync_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            
            _mockPropertyRepository.Setup(x => x.GetByIdAsync("nonexistent")).ReturnsAsync((Property?)null);

            
            var result = await _propertyService.GetPropertyByIdAsync("nonexistent");

            
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("no encontrada");
        }

        [Test]
        public async Task GetPropertyByIdAsync_ShouldReturnSuccess_WhenPropertyExists()
        {
            
            var property = new Property 
            { 
                Id = "1", 
                Name = "Test Property", 
                IdOwner = "owner1",
                Address = "Test Address",
                Price = 100000,
                Year = 2020
            };
            var owner = new Owner { Id = "owner1", Name = "Test Owner" };

            _mockPropertyRepository.Setup(x => x.GetByIdAsync("1")).ReturnsAsync(property);
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("owner1")).ReturnsAsync(owner);

            
            var result = await _propertyService.GetPropertyByIdAsync("1");

            
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Name.Should().Be("Test Property");
        }

        [Test]
        public async Task CreatePropertyAsync_ShouldReturnError_WhenOwnerDoesNotExist()
        {
            
            var propertyDto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                IdOwner = "nonexistent-owner",
                Year = 2023
            };

            _mockOwnerRepository.Setup(x => x.ExistsAsync("nonexistent-owner")).ReturnsAsync(false);

            
            var result = await _propertyService.CreatePropertyAsync(propertyDto);

            
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("propietario especificado no existe");
        }

        [Test]
        public async Task CreatePropertyAsync_ShouldReturnSuccess_WhenDataIsValid()
        {
            
            var propertyDto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                IdOwner = "valid-owner",
                Year = 2023
            };

            var createdProperty = new Property
            {
                Id = "new-id",
                Name = propertyDto.Name,
                Address = propertyDto.Address,
                Price = propertyDto.Price,
                IdOwner = propertyDto.IdOwner,
                Year = propertyDto.Year,
                CodeInternal = 123456
            };

            var owner = new Owner { Id = "valid-owner", Name = "Valid Owner" };

            _mockOwnerRepository.Setup(x => x.ExistsAsync("valid-owner")).ReturnsAsync(true);
            _mockPropertyRepository.Setup(x => x.CreateAsync(It.IsAny<Property>())).ReturnsAsync(createdProperty);
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("valid-owner")).ReturnsAsync(owner);

            
            var result = await _propertyService.CreatePropertyAsync(propertyDto);

            
            result.Success.Should().BeTrue();
            result.Data!.Name.Should().Be("Test Property");
            result.Data.OwnerName.Should().Be("Valid Owner");
        }

        [Test]
        public async Task UpdatePropertyAsync_ShouldReturnError_WhenPropertyNotFound()
        {
            
            var updateDto = new PropertyUpdateDto { Name = "Updated Name" };
            _mockPropertyRepository.Setup(x => x.GetByIdAsync("nonexistent")).ReturnsAsync((Property?)null);

            
            var result = await _propertyService.UpdatePropertyAsync("nonexistent", updateDto);

            
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("no encontrada");
        }

        [Test]
        public async Task DeletePropertyAsync_ShouldReturnError_WhenPropertyNotFound()
        {
            
            _mockPropertyRepository.Setup(x => x.ExistsAsync("nonexistent")).ReturnsAsync(false);

            
            var result = await _propertyService.DeletePropertyAsync("nonexistent");

            
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("no encontrada");
        }

        [Test]
        public async Task GetFilteredPropertiesAsync_ShouldReturnSuccess_WithValidFilters()
        {
            
            var filter = new PropertyFilterDto
            {
                Name = "Test",
                MinPrice = 50000,
                MaxPrice = 150000,
                Page = 1,
                PageSize = 10
            };

            var properties = new List<Property>
            {
                new Property 
                { 
                    Id = "1", 
                    Name = "Test Property", 
                    Price = 100000,
                    IdOwner = "owner1"
                }
            };
            var owner = new Owner { Id = "owner1", Name = "Test Owner" };

            _mockPropertyRepository.Setup(x => x.GetFilteredAsync(filter))
                .ReturnsAsync((properties, 1));
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("owner1")).ReturnsAsync(owner);

            
            var result = await _propertyService.GetFilteredPropertiesAsync(filter);

            
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(1);
            result.Total.Should().Be(1);
        }
    }
}