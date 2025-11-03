using Xunit;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Tests.Models;

public class VolunteerAssignmentTests
{
    [Fact]
    public void VolunteerAssignment_Create_ValidModel()
    {
        // Arrange
        var assignment = new VolunteerAssignment
        {
            Status = "Assigned",
            HoursContributed = 10.5m,
            AssignedDate = DateTime.Now
        };

        // Act & Assert
        Assert.NotNull(assignment);
        Assert.Equal("Assigned", assignment.Status);
        Assert.Equal(10.5m, assignment.HoursContributed);
    }

    [Fact]
    public void VolunteerAssignment_DefaultStatus_IsAssigned()
    {
        // Arrange
        var assignment = new VolunteerAssignment();

        // Act & Assert
        Assert.Equal("Assigned", assignment.Status);
    }

    [Theory]
    [InlineData("Assigned")]
    [InlineData("In Progress")]
    [InlineData("Completed")]
    [InlineData("Cancelled")]
    public void VolunteerAssignment_StatusAccepted(string status)
    {
        // Arrange
        var assignment = new VolunteerAssignment { Status = status };

        // Act & Assert
        Assert.Equal(status, assignment.Status);
    }

    [Fact]
    public void VolunteerAssignment_AssignedDate_IsSet()
    {
        // Arrange
        var assignment = new VolunteerAssignment();

        // Act & Assert
        Assert.True(assignment.AssignedDate <= DateTime.Now);
    }
}


