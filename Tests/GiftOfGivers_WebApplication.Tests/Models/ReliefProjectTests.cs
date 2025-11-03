using Xunit;
using GiftOfGivers_WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class ReliefProjectTests
{
    [Fact]
    public void ReliefProject_Create_ValidModel()
    {
        // Arrange
        var project = new ReliefProject
        {
            Name = "Flood Relief Project",
            Status = "Active"
        };

        // Act
        var validationResults = ValidateModel(project);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal("Flood Relief Project", project.Name);
        Assert.Equal("Active", project.Status);
    }

    [Fact]
    public void ReliefProject_NameIsRequired()
    {
        // Arrange
        var project = new ReliefProject
        {
            Status = "Active"
        };

        // Act
        var validationResults = ValidateModel(project);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(ReliefProject.Name)));
    }

    [Theory]
    [InlineData("Active")]
    [InlineData("Inactive")]
    [InlineData("Completed")]
    [InlineData("Cancelled")]
    public void ReliefProject_StatusAccepted(string status)
    {
        // Arrange
        var project = new ReliefProject
        {
            Name = "Test Project",
            Status = status
        };

        // Act
        var validationResults = ValidateModel(project);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal(status, project.Status);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}


