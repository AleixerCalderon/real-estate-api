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
    public class OwnerServiceTests
    {
        private Mock<IOwnerRepository> _mockOwnerRepository;
        private OwnerService _ownerService;

        [SetUp]
        public void Setup()
        {
            _mockOwnerRepository = new Mock<IOwnerRepository>();
            _ownerService = new OwnerService(_mockOwnerRepository.Object);
        }

        [Test]
        public async Task GetAllOwnersAsync_ShouldReturnSuccess_WhenOwnersExist()
        {
            
            var owners = new List<Owner>
            {
                new Owner 
                { 
                    Id = "1", 
                    Name = "Test Owner",
                    Address = "Test Address",
                    Phone = "+57 300 123 4567",
                    Birthday = new DateTime(1990, 1, 1)
                }
            };

            _mockOwnerRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(owners);

            
            var result = await _ownerService.GetAllOwnersAsync();

            
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(1);
            result.Data!.First().Name.Should().Be("Test Owner");
        }

        [Test]
        public async Task GetOwnerByIdAsync_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            
            _mockOwnerRepository.Setup(x => x.GetByIdAsync("nonexistent")).ReturnsAsync((Owner?)null);

            
            var result = await _ownerService.GetOwnerByIdAsync("nonexistent");

            
            result.Success.Should().BeFalse();
            result.Message.Should().Contain("no encontrado");
        }

        [Test]
        public async Task CreateOwnerAsync_ShouldReturnSuccess_WhenDataIsValid()
        {
            
            var ownerDto = new OwnerCreateDto
            {
                Name = "Test Owner",
                Address = "Test Address",
                Phone = "+57 300 123 4567",
                Birthday = new DateTime(1990, 01, 01)
            };

            var createdOwner = new Owner
            {
                Id = "new-id",
                Name = ownerDto.Name,
                Address = ownerDto.Address,
                Phone = ownerDto.Phone,
                Birthday = ownerDto.Birthday
            };

            _mockOwnerRepository.Setup(x => x.CreateAsync(It.IsAny<Owner>())).ReturnsAsync(createdOwner);

            
            var result = await _ownerService.CreateOwnerAsync(ownerDto);

            
            result.Success.Should().BeTrue();
            result.Data!.Name.Should().Be("Test Owner");
        }
    }
}