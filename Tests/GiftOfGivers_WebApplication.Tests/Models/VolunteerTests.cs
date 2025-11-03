using Xunit;
using GiftOfGivers_WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class VolunteerTests
{
    [Fact]
    public void Volunteer_FullName_ComputedCorrectly()
    {
        // Arrange
        var volunteer = new Volunteer
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var fullName = volunteer.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void Volunteer_RequiredFieldsValidation()
    {
        // Arrange
        var volunteer = new Volunteer();

        // Act
        var validationResults = ValidateModel(volunteer);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Volunteer.FirstName)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Volunteer.LastName)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Volunteer.Email)));
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Volunteer.Phone)));
    }

    [Fact]
    public void Volunteer_EmailFormatValidation()
    {
        // Arrange
        var volunteer = new Volunteer
        {
            FirstName = "Test",
            LastName = "User",
            Email = "invalid-email",
            Phone = "1234567890"
        };

        // Act
        var validationResults = ValidateModel(volunteer);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Volunteer.Email)));
    }

    [Fact]
    public void Volunteer_RegistrationDate_DefaultValue()
    {
        // Arrange & Act
        var volunteer = new Volunteer();

        // Assert
        Assert.True(volunteer.RegistrationDate <= DateTime.Now);
        Assert.True(volunteer.RegistrationDate >= DateTime.Now.AddSeconds(-1));
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
