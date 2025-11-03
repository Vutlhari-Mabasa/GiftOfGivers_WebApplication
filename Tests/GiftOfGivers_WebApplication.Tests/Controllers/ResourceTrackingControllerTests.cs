using Xunit;
using Microsoft.AspNetCore.Mvc;
using GiftOfGivers_WebApplication.Controllers;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftOfGivers_WebApplication.Tests.Controllers;

public class ResourceTrackingControllerTests
{
    private ApplicationDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Index_ReturnsViewWithResources()
    {
        // Arrange
        using var context = GetContext();
        var controller = new ResourceTrackingController(context);

        context.ResourceTracking.AddRange(
            new ResourceTracking { Name = "Resource A", Quantity = 10, Category = "Medical" },
            new ResourceTracking { Name = "Resource B", Quantity = 20, Category = "Food" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<ResourceTracking>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_ValidModel_ReturnsRedirectToIndex()
    {
        // Arrange
        using var context = GetContext();
        var controller = new ResourceTrackingController(context);

        var resource = new ResourceTracking
        {
            Name = "Test Resource",
            Quantity = 50,
            Category = "Medical",
            Priority = "High",
            Status = "Available"
        };

        // Act
        var result = await controller.Create(resource);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_SavesResource()
    {
        // Arrange
        using var context = GetContext();
        var controller = new ResourceTrackingController(context);

        var resource = new ResourceTracking
        {
            Name = "Bandages",
            Quantity = 100,
            Category = "Medical",
            Description = "Sterile bandages",
            Priority = "High"
        };

        // Act
        await controller.Create(resource);

        // Assert
        var savedResource = await context.ResourceTracking.FirstOrDefaultAsync();
        Assert.NotNull(savedResource);
        Assert.Equal("Bandages", savedResource.Name);
        Assert.Equal("Available", savedResource.Status);
    }

    [Fact]
    public async Task Delete_ValidId_RemovesResource()
    {
        // Arrange
        using var context = GetContext();
        var controller = new ResourceTrackingController(context);

        var resource = new ResourceTracking
        {
            Name = "To Delete",
            Quantity = 10,
            Category = "General"
        };

        context.ResourceTracking.Add(resource);
        await context.SaveChangesAsync();

        // Act
        var result = await controller.DeleteConfirmed(resource.ResourceID);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        var deletedResource = await context.ResourceTracking.FindAsync(resource.ResourceID);
        Assert.Null(deletedResource);
    }
}
