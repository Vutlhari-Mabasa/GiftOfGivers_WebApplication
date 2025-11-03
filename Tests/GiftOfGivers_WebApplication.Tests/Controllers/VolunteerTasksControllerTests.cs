using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GiftOfGivers_WebApplication.Controllers;
using GiftOfGivers_WebApplication.Data;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Tests.Controllers;

public class VolunteerTasksControllerTests
{
    private ApplicationDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Index_ReturnsViewWithTasks()
    {
        // Arrange
        using var context = GetContext();
        var controller = new VolunteerTasksController(context);

        context.VolunteerTasks.AddRange(
            new VolunteerTask { Title = "Task 1", Description = "Description 1", Status = "Open" },
            new VolunteerTask { Title = "Task 2", Description = "Description 2", Status = "In Progress" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<VolunteerTask>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_ValidModel_ReturnsRedirect()
    {
        // Arrange
        using var context = GetContext();
        var controller = new VolunteerTasksController(context);

        var task = new VolunteerTask
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = "High",
            Status = "Open",
            VolunteersNeeded = 5
        };

        // Act
        var result = await controller.Create(task);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Create_AssignsDefaultStatus()
    {
        // Arrange
        using var context = GetContext();
        var controller = new VolunteerTasksController(context);

        var task = new VolunteerTask
        {
            Title = "Test Task",
            Description = "Test Description"
        };

        // Act
        await controller.Create(task);

        // Assert
        var savedTask = await context.VolunteerTasks.FirstOrDefaultAsync();
        Assert.NotNull(savedTask);
        Assert.Equal("Open", savedTask.Status);
    }

    [Fact]
    public async Task Edit_ValidModel_UpdatesTask()
    {
        // Arrange
        using var context = GetContext();
        var controller = new VolunteerTasksController(context);

        var task = new VolunteerTask
        {
            Title = "Original Title",
            Description = "Original Description",
            Status = "Open"
        };
        context.VolunteerTasks.Add(task);
        await context.SaveChangesAsync();

        task.Title = "Updated Title";

        // Act
        var result = await controller.Edit(task.TaskID, task);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        var updatedTask = await context.VolunteerTasks.FindAsync(task.TaskID);
        Assert.Equal("Updated Title", updatedTask!.Title);
    }

    [Fact]
    public async Task Details_InvalidId_ReturnsNotFound()
    {
        // Arrange
        using var context = GetContext();
        var controller = new VolunteerTasksController(context);

        // Act
        var result = await controller.Details(9999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}


