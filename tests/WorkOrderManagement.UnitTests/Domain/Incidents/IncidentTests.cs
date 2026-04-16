using FluentAssertions;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.UnitTests.Domain.Incidents;

public class IncidentTests
{
    [Fact]
    public void Constructor_Should_Create_Incident_With_Open_Status()
    {
        // Arrange
        var title = "Incident1";
        var description = "Incident description";
        var buildingId = Guid.NewGuid();
        var category = "Test category";
        var priority = IncidentPriority.Low;
        var reportedByUserId = Guid.NewGuid();

        // Act
        var incident = new Incident(title, description, buildingId, category, priority, reportedByUserId);

        // Assert
        incident.Status.Should().Be(IncidentStatus.Open);
    }

    [Fact]
    public void UpdatePriority_Should_Change_Priority()
    {
        // Arrange
        var incident = new Incident(
            "Incident1",
            "Incident description",
            Guid.NewGuid(),
            "Test category",
            IncidentPriority.Low,
            Guid.NewGuid());

        // Act
        incident.UpdatePriority(IncidentPriority.High);

        // Assert
        incident.Priority.Should().Be(IncidentPriority.High);
    }

    [Fact]
    public void UpdateStatus_Should_Throw_When_Incident_Is_Closed()
    {
        // Arrange
        var incident = new Incident(
            "Incident1",
            "Incident description",
            Guid.NewGuid(),
            "Test category",
            IncidentPriority.Low,
            Guid.NewGuid());

        incident.UpdateStatus(IncidentStatus.Closed);

        // Act
        Action act = () => incident.UpdateStatus(IncidentStatus.Open);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("A closed incident cannot be changed.");
    }

    [Fact]
    public void UpdateStatus_Should_Throw_When_Resolved_Incident_Returns_To_Open()
    {
        // Arrange
        var incident = new Incident(
            "Incident1",
            "Incident description",
            Guid.NewGuid(),
            "Test category",
            IncidentPriority.Low,
            Guid.NewGuid());

        incident.UpdateStatus(IncidentStatus.Resolved);

        // Act
        Action act = () => incident.UpdateStatus(IncidentStatus.Open);

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("A resolved incident cannot return to open.");
    }
}