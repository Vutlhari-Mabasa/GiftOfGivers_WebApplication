using Xunit;
using GiftOfGivers_WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class IncidentReportTests
{
    [Fact]
    public void IncidentReport_Create_ValidModel()
    {
        // Arrange
        var incident = new IncidentReport
        {
            Name = "Hurricane Alpha",
            Type = "Natural Disaster",
            Location = "Coastal Region",
            StartDate = DateTime.Now,
            SeverityLevel = "Critical",
            EstimatedAffectedPeople = 10000,
            Status = "Active"
        };

        // Act
        var validationResults = ValidateModel(incident);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal("Hurricane Alpha", incident.Name);
        Assert.Equal("Critical", incident.SeverityLevel);
    }

    [Fact]
    public void IncidentReport_RequiredFields()
    {
        // Arrange
        var incident = new IncidentReport();

        // Act
        var validationResults = ValidateModel(incident);

        // Assert - All fields have defaults, so only Name, Type, and Location will fail
        // StartDate has a default value (DateTime.Now) so it won't fail validation
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(IncidentReport.Name)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(IncidentReport.Type)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(IncidentReport.Location)));
    }

    [Fact]
    public void IncidentReport_CreatedAt_DefaultValue()
    {
        // Arrange
        var incident = new IncidentReport
        {
            Name = "Test",
            Type = "Test",
            Location = "Test",
            StartDate = DateTime.Now
        };

        // Act
        ValidateModel(incident);

        // Assert
        Assert.True(incident.CreatedAt <= DateTime.Now);
    }

    [Theory]
    [InlineData("Low")]
    [InlineData("Medium")]
    [InlineData("High")]
    [InlineData("Critical")]
    public void IncidentReport_SeverityLevelAccepted(string severity)
    {
        // Arrange
        var incident = new IncidentReport
        {
            Name = "Test",
            Type = "Test",
            Location = "Test",
            StartDate = DateTime.Now,
            SeverityLevel = severity
        };

        // Act
        var validationResults = ValidateModel(incident);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal(severity, incident.SeverityLevel);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
