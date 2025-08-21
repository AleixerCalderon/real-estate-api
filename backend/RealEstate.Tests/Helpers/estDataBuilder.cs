using RealEstate.Domain.Entities;
using RealEstate.Application.DTOs;

namespace RealEstate.Tests.Helpers
{
    public static class TestDataBuilder
    {
        public static Property CreateValidProperty(string? id = null, string? ownerId = null)
        {
            return new Property
            {
                Id = id ?? "test-property-id",
                Name = "Test Property",
                Address = "123 Test Street, Test City",
                Price = 250000,
                CodeInternal = 123456,
                Year = 2022,
                IdOwner = ownerId ?? "test-owner-id",
                Image = "https://example.com/image.jpg"
            };
        }

        public static Owner CreateValidOwner(string? id = null)
        {
            return new Owner
            {
                Id = id ?? "test-owner-id",
                Name = "Test Owner",
                Address = "456 Owner Street, Owner City",
                Phone = "+57 300 123 4567",
                Birthday = new DateTime(1985, 6, 15)
            };
        }

        public static PropertyCreateDto CreateValidPropertyCreateDto(string? ownerId = null)
        {
            return new PropertyCreateDto
            {
                Name = "New Test Property",
                Address = "789 New Street, New City",
                Price = 300000,
                IdOwner = ownerId ?? "test-owner-id",
                Year = 2023,
                Image = "https://example.com/new-image.jpg"
            };
        }

        public static OwnerCreateDto CreateValidOwnerCreateDto()
        {
            return new OwnerCreateDto
            {
                Name = "New Test Owner",
                Address = "321 New Owner Street, New Owner City",
                Phone = "+57 301 987 6543",
                Birthday = new DateTime(1990,03,20)
            };
        }

        public static PropertyFilterDto CreatePropertyFilter(
            string? name = null,
            string? address = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int page = 1,
            int pageSize = 10)
        {
            return new PropertyFilterDto
            {
                Name = name,
                Address = address,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Page = page,
                PageSize = pageSize
            };
        }

        public static List<Property> CreatePropertyList(int count = 3)
        {
            var properties = new List<Property>();
            for (int i = 1; i <= count; i++)
            {
                properties.Add(new Property
                {
                    Id = $"property-{i}",
                    Name = $"Property {i}",
                    Address = $"Address {i}",
                    Price = 100000 * i,
                    CodeInternal = 100000 + i,
                    Year = 2020 + i,
                    IdOwner = $"owner-{i}",
                    Image = $"image-{i}.jpg"
                });
            }
            return properties;
        }

        public static List<Owner> CreateOwnerList(int count = 3)
        {
            var owners = new List<Owner>();
            for (int i = 1; i <= count; i++)
            {
                owners.Add(new Owner
                {
                    Id = $"owner-{i}",
                    Name = $"Owner {i}",
                    Address = $"Owner Address {i}",
                    Phone = $"+57 30{i} 123 456{i}",
                    Birthday = new DateTime(1980 + i, i, i)
                });
            }
            return owners;
        }
    }
}