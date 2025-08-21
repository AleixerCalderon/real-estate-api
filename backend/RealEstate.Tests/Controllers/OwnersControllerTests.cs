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
    public class OwnersControllerTests
    {
        private Mock<IOwnerService> _mockOwnerService;
        private Mock<ILogger<OwnersController>> _mockLogger;
        private OwnersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockOwnerService = new Mock<IOwnerService>();
            _mockLogger = new Mock<ILogger<OwnersController>>();
            _controller = new OwnersController(_mockOwnerService.Object, _mockLogger.Object);
        }

        [Test]
        public async Task GetAllOwners_ShouldReturnOk_WhenServiceReturnsSuccess()
        {
            // AAB (20 08 2025)
            var owners = new List<OwnerDto>
            {
                new OwnerDto { Id = "1", Name = "Test Owner" }
            };
            var response = ApiResponseDto<IEnumerable<OwnerDto>>.SuccessResponse(owners);

            _mockOwnerService.Setup(x => x.GetAllOwnersAsync()).ReturnsAsync(response);

            // AAB (20 08 2025)
            var result = await _controller.GetAllOwners();

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().Be(response);
        }

        [Test]
        public async Task GetOwner_ShouldReturnOk_WhenOwnerExists()
        {
            var owner = new OwnerDto { Id = "1", Name = "Test Owner" };
            var response = ApiResponseDto<OwnerDto>.SuccessResponse(owner);
            _mockOwnerService.Setup(x => x.GetOwnerByIdAsync("1")).ReturnsAsync(response);

            var result = await _controller.GetOwner("1");

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetOwner_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
            var response = ApiResponseDto<OwnerDto>.ErrorResponse("Propietario no encontrado");
            _mockOwnerService.Setup(x => x.GetOwnerByIdAsync("nonexistent")).ReturnsAsync(response);

            var result = await _controller.GetOwner("nonexistent");

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task CreateOwner_ShouldReturnCreated_WhenValidData()
        {
            var createDto = new OwnerCreateDto
            {
                Name = "Test Owner",
                Address = "Test Address",
                Phone = "+57 300 123 4567",
                Birthday = new DateTime(1990,01,01)
            };

            var createdOwner = new OwnerDto { Id = "new-id", Name = "Test Owner" };
            var response = ApiResponseDto<OwnerDto>.SuccessResponse(createdOwner);

            _mockOwnerService.Setup(x => x.CreateOwnerAsync(createDto)).ReturnsAsync(response);

            var result = await _controller.CreateOwner(createDto);

            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Test]
        public async Task CreateOwner_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Name is required");
            var createDto = new OwnerCreateDto();

            var result = await _controller.CreateOwner(createDto);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task UpdateOwner_ShouldReturnOk_WhenValidData()
        {
            var updateDto = new OwnerUpdateDto { Name = "Updated Owner" };
            var updatedOwner = new OwnerDto { Id = "1", Name = "Updated Owner" };
            var response = ApiResponseDto<OwnerDto>.SuccessResponse(updatedOwner);

            _mockOwnerService.Setup(x => x.UpdateOwnerAsync("1", updateDto)).ReturnsAsync(response);

            var result = await _controller.UpdateOwner("1", updateDto);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturnOk_WhenOwnerExists()
        {
          
            var response = ApiResponseDto<bool>.SuccessResponse(true);
            _mockOwnerService.Setup(x => x.DeleteOwnerAsync("1")).ReturnsAsync(response);

            var result = await _controller.DeleteOwner("1");

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task DeleteOwner_ShouldReturnNotFound_WhenOwnerDoesNotExist()
        {
          
            var response = ApiResponseDto<bool>.ErrorResponse("Propietario no encontrado");
            _mockOwnerService.Setup(x => x.DeleteOwnerAsync("nonexistent")).ReturnsAsync(response);

            var result = await _controller.DeleteOwner("nonexistent");

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}