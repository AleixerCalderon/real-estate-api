using NUnit.Framework;
using FluentAssertions;
using RealEstate.Application.DTOs;

namespace RealEstate.Tests.DTOs
{
    [TestFixture]
    public class PropertyDtoTests
    {
        [Test]
        public void PropertyDto_ShouldInitializeWithDefaultValues()
        {
            
            var propertyDto = new PropertyDto();

            
            propertyDto.Id.Should().Be(string.Empty);
            propertyDto.Name.Should().Be(string.Empty);
            propertyDto.Address.Should().Be(string.Empty);
            propertyDto.Price.Should().Be(0);
            propertyDto.Image.Should().Be(string.Empty);
            propertyDto.OwnerName.Should().Be(string.Empty);
            propertyDto.Year.Should().Be(0);
            propertyDto.CodeInternal.Should().Be(0);
        }

        [Test]
        public void PropertyCreateDto_ShouldSetPropertiesCorrectly()
        {
             
            var createDto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                IdOwner = "owner-id",
                Year = 2023,
                Image = "test-image.jpg"
            };

            
            createDto.Name.Should().Be("Test Property");
            createDto.Address.Should().Be("Test Address");
            createDto.Price.Should().Be(100000);
            createDto.IdOwner.Should().Be("owner-id");
            createDto.Year.Should().Be(2023);
            createDto.Image.Should().Be("test-image.jpg");
        }

        [Test]
        public void PropertyFilterDto_ShouldInitializeWithDefaultValues()
        {
            
            var filterDto = new PropertyFilterDto();

            
            filterDto.Name.Should().BeNull();
            filterDto.Address.Should().BeNull();
            filterDto.MinPrice.Should().BeNull();
            filterDto.MaxPrice.Should().BeNull();
            filterDto.Page.Should().Be(1);
            filterDto.PageSize.Should().Be(10);
        }

        [Test]
        public void ApiResponseDto_ShouldCreateSuccessResponse()
        {
            
            var data = "test data";
            var message = "Success";

            
            var response = ApiResponseDto<string>.SuccessResponse(data, message);

            
            response.Success.Should().BeTrue();
            response.Message.Should().Be(message);
            response.Data.Should().Be(data);
        }

        [Test]
        public void ApiResponseDto_ShouldCreateErrorResponse()
        {
            
            var message = "Error occurred";

            
            var response = ApiResponseDto<string>.ErrorResponse(message);

            
            response.Success.Should().BeFalse();
            response.Message.Should().Be(message);
            response.Data.Should().BeNull();
        }

        [Test]
        public void ApiResponseDto_ShouldCreatePagedResponse()
        {
            
            var data = new List<string> { "item1", "item2" };
            var total = 100;
            var page = 2;
            var pageSize = 10;

            
            var response = ApiResponseDto<List<string>>.PagedResponse(data, total, page, pageSize);

            
            response.Success.Should().BeTrue();
            response.Data.Should().BeEquivalentTo(data);
            response.Total.Should().Be(total);
            response.Page.Should().Be(page);
            response.PageSize.Should().Be(pageSize);
        }
    }
}