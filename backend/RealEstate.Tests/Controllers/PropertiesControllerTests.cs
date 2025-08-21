using NUnit.Framework;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealEstate.API.Controllers;
using RealEstate.Application.Interfaces;
using RealEstate.Application.DTOs;

namespace RealEstate.Tests.Controllers
{
    [TestFixture]
    public class PropertiesControllerTests
    {
        private Mock<IPropertyService> _mockPropertyService;
        private Mock<ILogger<PropertiesController>> _mockLogger;
        private PropertiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockPropertyService = new Mock<IPropertyService>();
            _mockLogger = new Mock<ILogger<PropertiesController>>();
            _controller = new PropertiesController(_mockPropertyService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetAllProperties_ShouldReturnOk_WhenServiceReturnsSuccess()
        {
            
            var properties = new List<PropertyDto>
            {
                new PropertyDto { Id = "1", Name = "Test Property" }
            };
            var response = ApiResponseDto<IEnumerable<PropertyDto>>.SuccessResponse(properties);

            _mockPropertyService.Setup(x => x.GetAllPropertiesAsync()).ReturnsAsync(response);

            
            var result = await _controller.GetAllProperties();

            
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().Be(response);
        }

        [Test]
        public async Task GetAllProperties_ShouldReturnInternalServerError_WhenServiceFails()
        {
            
            var response = ApiResponseDto<IEnumerable<PropertyDto>>.ErrorResponse("Database error");
            _mockPropertyService.Setup(x => x.GetAllPropertiesAsync()).ReturnsAsync(response);

            
            var result = await _controller.GetAllProperties();

            
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult!.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task GetProperty_ShouldReturnOk_WhenPropertyExists()
        {
            
            var property = new PropertyDto { Id = "1", Name = "Test Property" };
            var response = ApiResponseDto<PropertyDto>.SuccessResponse(property);
            _mockPropertyService.Setup(x => x.GetPropertyByIdAsync("1")).ReturnsAsync(response);

            
            var result = await _controller.GetProperty("1");

            
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetProperty_ShouldReturnNotFound_WhenPropertyDoesNotExist()
        {
            
            var response = ApiResponseDto<PropertyDto>.ErrorResponse("Propiedad no encontrada");
            _mockPropertyService.Setup(x => x.GetPropertyByIdAsync("nonexistent")).ReturnsAsync(response);

            
            var result = await _controller.GetProperty("nonexistent");

            
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task SearchProperties_ShouldReturnOk_WhenValidParameters()
        {
            
            var properties = new List<PropertyDto>();
            var response = ApiResponseDto<IEnumerable<PropertyDto>>.PagedResponse(properties, 0, 1, 10);
            _mockPropertyService.Setup(x => x.GetFilteredPropertiesAsync(It.IsAny<PropertyFilterDto>()))
                .ReturnsAsync(response);

            PropertyFilterDto filter = new PropertyFilterDto();
            filter.MinPrice = 100000;
            filter.MaxPrice = 500000;
            filter.Page = 1;
            filter.PageSize = 10;
            filter.Name = "test";
            filter.Address = "address";
            
            var result = await _controller.SearchProperties( filter);

            
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task SearchProperties_ShouldReturnBadRequest_WhenInvalidPagination()
        {
             
            PropertyFilterDto filter = new PropertyFilterDto();            
            filter.Page = 0;
            filter.PageSize = 0;           
            var result = await _controller.SearchProperties(filter);

            
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task CreateProperty_ShouldReturnCreated_WhenValidData()
        {
            
            var createDto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                IdOwner = "owner-id",
                Year = 2023
            };

            var createdProperty = new PropertyDto { Id = "new-id", Name = "Test Property" };
            var response = ApiResponseDto<PropertyDto>.SuccessResponse(createdProperty);

            _mockPropertyService.Setup(x => x.CreatePropertyAsync(createDto)).ReturnsAsync(response);

            
            var result = await _controller.CreateProperty(createDto);

            
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult!.Value.Should().Be(response);
        }

        [Test]
        public async Task CreateProperty_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            
            _controller.ModelState.AddModelError("Name", "Name is required");
            var createDto = new PropertyCreateDto();

            
            var result = await _controller.CreateProperty(createDto);

            
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task UpdateProperty_ShouldReturnOk_WhenValidData()
        {
            
            var updateDto = new PropertyUpdateDto { Name = "Updated Property" };
            var updatedProperty = new PropertyDto { Id = "1", Name = "Updated Property" };
            var response = ApiResponseDto<PropertyDto>.SuccessResponse(updatedProperty);

            _mockPropertyService.Setup(x => x.UpdatePropertyAsync("1", updateDto)).ReturnsAsync(response);

            
            var result = await _controller.UpdateProperty("1", updateDto);

            
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task DeleteProperty_ShouldReturnOk_WhenPropertyExists()
        {
            
            var response = ApiResponseDto<bool>.SuccessResponse(true);
            _mockPropertyService.Setup(x => x.DeletePropertyAsync("1")).ReturnsAsync(response);

            
            var result = await _controller.DeleteProperty("1");

            
            result.Result.Should().BeOfType<OkObjectResult>();
        }
    }
}