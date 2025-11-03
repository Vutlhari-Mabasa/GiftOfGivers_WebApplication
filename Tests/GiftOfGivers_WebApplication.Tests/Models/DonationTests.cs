using Xunit;
using GiftOfGivers_WebApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class DonationTests
{
    [Fact]
    public void Donation_Create_ValidModel()
    {
        // Arrange
        var donation = new Donation
        {
            Amount = 5000.00m,
            Type = "Financial",
            Date = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.Empty(validationResults);
        Assert.Equal(5000.00m, donation.Amount);
        Assert.Equal("Financial", donation.Type);
    }

    [Fact]
    public void Donation_AmountIsRequired()
    {
        // Arrange
        var donation = new Donation
        {
            Type = "Financial",
            Date = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Donation.Amount)));
    }

    [Fact]
    public void Donation_DateIsRequired()
    {
        // Arrange
        var donation = new Donation
        {
            Amount = 1000m,
            Type = "Financial"
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.NotEmpty(validationResults);
        Assert.Contains(validationResults, r => r.MemberNames.Contains(nameof(Donation.Date)));
    }

    [Theory]
    [InlineData(100.50)]
    [InlineData(999999.99)]
    [InlineData(0.01)]
    public void Donation_PositiveAmountsAccepted(decimal amount)
    {
        // Arrange
        var donation = new Donation
        {
            Amount = amount,
            Type = "Financial",
            Date = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(donation);

        // Assert
        Assert.Empty(validationResults);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }
}
