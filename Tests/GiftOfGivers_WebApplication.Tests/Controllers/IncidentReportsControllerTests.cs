using Xunit;
using Microsoft.AspNetCore.Mvc;
using GiftOfGivers_WebApplication.Controllers;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GiftOfGivers_WebApplication.Tests.Controllers;

public class IncidentReportsControllerTests
{
    private ApplicationDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Index_ReturnsViewWithIncidentReports()
    {
        // Arrange
        using var context = GetContext();
        var controller = new IncidentReportsController(context);

        context.IncidentReports.AddRange(
            new IncidentReport { Name = "Flood A", Type = "Flood", Location = "Location A", StartDate = DateTime.Now },
            new IncidentReport { Name = "Fire B", Type = "Fire", Location = "Location B", StartDate = DateTime.Now }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<IncidentReport>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_ValidModel_ReturnsRedirectToIndex()
    {
        // Arrange
        using var context = GetContext();
        var controller = new IncidentReportsController(context);

        var incident = new IncidentReport
        {
            Name = "Test Incident",
            Type = "Earthquake",
            Location = "Test Location",
            StartDate = DateTime.Now,
            SeverityLevel = "High"
        };

        // Act
        var result = await controller.Create(incident);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_SavesIncidentReport()
    {
        // Arrange
        using var context = GetContext();
        var controller = new IncidentReportsController(context);

        var incident = new IncidentReport
        {
            Name = "Save Test",
            Type = "Flood",
            Location = "Test Location",
            StartDate = DateTime.Now
        };

        // Act
        await controller.Create(incident);

        // Assert
        var savedIncident = await context.IncidentReports.FirstOrDefaultAsync();
        Assert.NotNull(savedIncident);
        Assert.Equal("Save Test", savedIncident.Name);
    }

    [Fact]
    public async Task Details_ValidId_ReturnsView()
    {
        // Arrange
        using var context = GetContext();
        var controller = new IncidentReportsController(context);

        var incident = new IncidentReport
        {
            Name = "Details Test",
            Type = "Fire",
            Location = "Test Location",
            StartDate = DateTime.Now
        };

        context.IncidentReports.Add(incident);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Details(incident.IncidentID);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<IncidentReport>(viewResult.Model);
        Assert.Equal(incident.IncidentID, model.IncidentID);
    }

    [Fact]
    public async Task Details_InvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = GetContext();
        var controller = new IncidentReportsController(context);

        // Act
        var result = await controller.Details(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
