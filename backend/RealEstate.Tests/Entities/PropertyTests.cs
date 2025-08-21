using NUnit.Framework;
using FluentAssertions;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.Entities
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void Property_ShouldInitializeWithDefaultValues()
        {
            
            var property = new Property();

            
            property.Id.Should().Be(string.Empty);
            property.Name.Should().Be(string.Empty);
            property.Address.Should().Be(string.Empty);
            property.Price.Should().Be(0);
            property.CodeInternal.Should().Be(0);
            property.Year.Should().Be(0);
            property.IdOwner.Should().Be(string.Empty);
            property.Image.Should().Be(string.Empty);
        }

        [Test]
        public void Property_ShouldSetPropertiesCorrectly()
        {
           
            var property = new Property
            {
                Id = "test-id",
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                CodeInternal = 123456,
                Year = 2023,
                IdOwner = "owner-id",
                Image = "test-image.jpg"
            };

            
            property.Id.Should().Be("test-id");
            property.Name.Should().Be("Test Property");
            property.Address.Should().Be("Test Address");
            property.Price.Should().Be(100000);
            property.CodeInternal.Should().Be(123456);
            property.Year.Should().Be(2023);
            property.IdOwner.Should().Be("owner-id");
            property.Image.Should().Be("test-image.jpg");
        }

        [Test]
        public void Owner_ShouldInitializeWithDefaultValues()
        {
            
            var owner = new Owner();

            
            owner.Id.Should().Be(string.Empty);
            owner.Name.Should().Be(string.Empty);
            owner.Address.Should().Be(string.Empty);
            owner.Phone.Should().Be(string.Empty);
            owner.Birthday.Should().Be(default(DateTime));
        }

        [Test]
        public void Owner_ShouldSetPropertiesCorrectly()
        {
            
            var birthday = new DateTime(1990, 1, 1);

            
            var owner = new Owner
            {
                Id = "owner-id",
                Name = "Test Owner",
                Address = "Test Address",
                Phone = "+57 300 123 4567",
                Birthday = birthday
            };

            
            owner.Id.Should().Be("owner-id");
            owner.Name.Should().Be("Test Owner");
            owner.Address.Should().Be("Test Address");
            owner.Phone.Should().Be("+57 300 123 4567");
            owner.Birthday.Should().Be(birthday);
        }
    }
}