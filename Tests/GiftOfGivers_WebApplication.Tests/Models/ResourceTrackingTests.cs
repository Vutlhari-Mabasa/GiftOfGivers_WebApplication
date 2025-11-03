using Xunit;
using GiftOfGivers_WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class ResourceTrackingTests
{
    [Fact]
    public void ResourceTracking_Create_ValidModel()
    {
        // Arrange
        var resource = new ResourceTracking
        {
            Name = "Medical Supplies",
            Quantity = 100,
            Unit = "boxes",
            Category = "Medical",
            Description = "Bandages and first aid kits",
            DonatedBy = "ABC Company",
            Priority = "High",
            Status = "Available"
        };

        // Act
        var validationResults = ValidateModel(resource);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal("Medical Supplies", resource.Name);
        Assert.Equal(100, resource.Quantity);
        Assert.Equal("High", resource.Priority);
    }

    [Fact]
    public void ResourceTracking_NameIsRequired()
    {
        // Arrange
        var resource = new ResourceTracking
        {
            Quantity = 100
        };

        // Act
        var validationResults = ValidateModel(resource);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(ResourceTracking.Name)));
    }

    [Fact]
    public void ResourceTracking_QuantityIsRequired()
    {
        // Arrange
        var resource = new ResourceTracking
        {
            Name = "Test Resource"
        };

        // Act
        var validationResults = ValidateModel(resource);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(ResourceTracking.Quantity)));
    }

    [Theory]
    [InlineData("Medical")]
    [InlineData("Food")]
    [InlineData("Clothing")]
    [InlineData("General")]
    public void ResourceTracking_CategoryAccepted(string category)
    {
        // Arrange
        var resource = new ResourceTracking
        {
            Name = "Test",
            Quantity = 10,
            Category = category
        };

        // Act
        var validationResults = ValidateModel(resource);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal(category, resource.Category);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
