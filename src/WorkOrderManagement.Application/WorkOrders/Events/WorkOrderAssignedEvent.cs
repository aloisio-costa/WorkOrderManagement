namespace WorkOrderManagement.Application.WorkOrders.Events;

public sealed record WorkOrderAssignedEvent(
    Guid WorkOrderId,
    Guid TechnicianId,
    DateTime AssignedAtUtc
);