using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Dtos;

public class WorkOrderDto
{
    public Guid Id { get; init; }
    public Guid IncidentId { get; init; }
    public Guid? AssignedTechnicianId { get; init; }
    public WorkOrderStatus Status { get; init; }
    public DateTime? DueDateUtc { get; init; }
    public string? Notes { get; init; }
    public DateTime? CompletedAtUtc { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}