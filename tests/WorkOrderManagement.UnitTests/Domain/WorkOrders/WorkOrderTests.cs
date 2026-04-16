using FluentAssertions;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.UnitTests.Domain.WorkOrders;

public class WorkOrderTests
{
    [Fact]
    public void Constructor_Should_Create_WorkOrder_With_Created_Status()
    {
        // Arrange
        var incidentId = Guid.NewGuid();

        // Act
        var workOrder = new WorkOrder(incidentId, DateTime.UtcNow.AddDays(1), "Initial note");

        // Assert
        workOrder.IncidentId.Should().Be(incidentId);
        workOrder.Status.Should().Be(WorkOrderStatus.Created);
        workOrder.AssignedTechnicianId.Should().BeNull();
        workOrder.Notes.Should().Be("Initial note");
    }

    [Fact]
    public void AssignTechnician_Should_Set_Technician_And_Change_Status_To_Assigned()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);
        var technicianId = Guid.NewGuid();

        // Act
        workOrder.AssignTechnician(technicianId);

        // Assert
        workOrder.AssignedTechnicianId.Should().Be(technicianId);
        workOrder.Status.Should().Be(WorkOrderStatus.Assigned);
    }

    [Fact]
    public void Start_Should_Throw_When_WorkOrder_Is_Not_Assigned()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);

        // Act
        Action act = () => workOrder.Start();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("A work order must be assigned before it can start.");
    }

    [Fact]
    public void Start_Should_Change_Status_To_InProgress_When_Assigned()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);
        workOrder.AssignTechnician(Guid.NewGuid());

        // Act
        workOrder.Start();

        // Assert
        workOrder.Status.Should().Be(WorkOrderStatus.InProgress);
    }

    [Fact]
    public void Complete_Should_Throw_When_WorkOrder_Is_Not_InProgress()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);
        workOrder.AssignTechnician(Guid.NewGuid());

        // Act
        Action act = () => workOrder.Complete();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Only work orders in progress can be completed.");
    }

    [Fact]
    public void Complete_Should_Set_Status_To_Completed_And_Set_CompletedAtUtc()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);
        workOrder.AssignTechnician(Guid.NewGuid());
        workOrder.Start();

        // Act
        workOrder.Complete();

        // Assert
        workOrder.Status.Should().Be(WorkOrderStatus.Completed);
        workOrder.CompletedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void Cancel_Should_Throw_When_WorkOrder_Is_Already_Completed()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);
        workOrder.AssignTechnician(Guid.NewGuid());
        workOrder.Start();
        workOrder.Complete();

        // Act
        Action act = () => workOrder.Cancel();

        // Assert
        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Completed work orders cannot be cancelled.");
    }

    [Fact]
    public void Cancel_Should_Set_Status_To_Cancelled_When_Not_Completed()
    {
        // Arrange
        var workOrder = new WorkOrder(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), null);

        // Act
        workOrder.Cancel();

        // Assert
        workOrder.Status.Should().Be(WorkOrderStatus.Cancelled);
    }
}