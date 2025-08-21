using NUnit.Framework;
using FluentAssertions;
using RealEstate.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Tests.Integration
{
    [TestFixture]
    public class ValidationTests
    {
        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Test]
        public void PropertyCreateDto_ShouldFailValidation_WhenRequiredFieldsAreMissing()
        {
            
            var dto = new PropertyCreateDto();

            
            var validationResults = ValidateModel(dto);

            
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Name"));
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Address"));
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("IdOwner"));
        }

        [Test]
        public void PropertyCreateDto_ShouldPassValidation_WhenAllRequiredFieldsAreProvided()
        {
            
            var dto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 100000,
                IdOwner = "owner-id",
                Year = 2023
            };

            
            var validationResults = ValidateModel(dto);

            
            validationResults.Should().BeEmpty();
        }

        [Test]
        public void PropertyCreateDto_ShouldFailValidation_WhenPriceIsZeroOrNegative()
        {
            
            var dto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address",
                Price = 0,
                IdOwner = "owner-id",
                Year = 2023
            };

            
            var validationResults = ValidateModel(dto);

            
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Price"));
        }

        [Test]
        public void PropertyCreateDto_ShouldFailValidation_WhenYearIsOutOfRange()
        {
            
            var dto = new PropertyCreateDto
            {
                Name = "Test Property",
                Address = "Test Address", 
                Price = 100000,
                IdOwner = "owner-id",
                Year = 1800 
            };

            
            var validationResults = ValidateModel(dto);

            
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Year"));
        }

        [Test]
        public void OwnerCreateDto_ShouldFailValidation_WhenRequiredFieldsAreMissing()
        {
            
            var dto = new OwnerCreateDto();

            
            var validationResults = ValidateModel(dto);

            
            validationResults.Should().NotBeEmpty();
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Name"));
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Address"));
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Phone"));
            validationResults.Should().Contain(vr => vr.MemberNames.Contains("Birthday"));
        }

        [Test]
        public void OwnerCreateDto_ShouldPassValidation_WhenAllFieldsAreValid()
        {
            
            var dto = new OwnerCreateDto
            {
                Name = "Test Owner",
                Address = "Test Address",
                Phone = "+57 300 123 4567",
                Birthday = new DateTime(1990,01,01)
            };

            
            var validationResults = ValidateModel(dto);

            
            validationResults.Should().BeEmpty();
        }
    }
}
