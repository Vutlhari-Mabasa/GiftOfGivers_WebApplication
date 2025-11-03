using Xunit;
using GiftOfGivers_WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class VolunteerTaskTests
{
    [Fact]
    public void VolunteerTask_Create_ValidModel()
    {
        // Arrange
        var task = new VolunteerTask
        {
            Title = "Distribution Task",
            Description = "Help distribute supplies",
            Priority = "High",
            Status = "Open",
            VolunteersNeeded = 10
        };

        // Act
        var validationResults = ValidateModel(task);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal("Distribution Task", task.Title);
        Assert.Equal("High", task.Priority);
    }

    [Fact]
    public void VolunteerTask_TitleIsRequired()
    {
        // Arrange
        var task = new VolunteerTask
        {
            Description = "Test Description"
        };

        // Act
        var validationResults = ValidateModel(task);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(VolunteerTask.Title)));
    }

    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    [InlineData("Critical")]
    public void VolunteerTask_PriorityAccepted(string priority)
    {
        // Arrange
        var task = new VolunteerTask
        {
            Title = "Test Task",
            Priority = priority
        };

        // Act
        var validationResults = ValidateModel(task);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal(priority, task.Priority);
    }

    [Fact]
    public void VolunteerTask_DefaultStatus_IsOpen()
    {
        // Arrange
        var task = new VolunteerTask { Title = "Test" };

        // Act & Assert
        Assert.Equal("Open", task.Status);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}


