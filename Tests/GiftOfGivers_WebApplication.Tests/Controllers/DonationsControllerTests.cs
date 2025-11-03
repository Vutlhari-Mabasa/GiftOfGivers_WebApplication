using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfGivers_WebApplication.Controllers;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Tests.Controllers;

public class DonationsControllerTests
{
    private ApplicationDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Index_ReturnsViewWithDonations()
    {
        // Arrange
        using var context = GetContext();
        var controller = new DonationsController(context);

        context.Donations.AddRange(
            new Donation { Amount = 1000, Type = "Financial", Date = DateTime.Now },
            new Donation { Amount = 5000, Type = "In-Kind", Date = DateTime.Now }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Donation>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_ValidDonation_SavesSuccessfully()
    {
        // Arrange
        using var context = GetContext();
        var controller = new DonationsController(context);

        var donation = new Donation
        {
            Amount = 2500.50m,
            Type = "Financial",
            Date = DateTime.Now
        };

        // Act
        var result = await controller.Create(donation);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        var savedDonation = await context.Donations.FirstOrDefaultAsync();
        Assert.NotNull(savedDonation);
        Assert.Equal(2500.50m, savedDonation.Amount);
    }

    [Fact]
    public async Task Delete_ValidId_RemovesDonation()
    {
        // Arrange
        using var context = GetContext();
        var controller = new DonationsController(context);

        var donation = new Donation { Amount = 1000, Type = "Financial", Date = DateTime.Now };
        context.Donations.Add(donation);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteConfirmed(donation.DonationID);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        var deletedDonation = await context.Donations.FindAsync(donation.DonationID);
        Assert.Null(deletedDonation);
    }
}


