using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Dtos;

public static class WorkOrderMappings
{
    public static WorkOrderDto ToDto(this WorkOrder workOrder)
    {
        return new WorkOrderDto
        {
            Id = workOrder.Id,
            IncidentId = workOrder.IncidentId,
            AssignedTechnicianId = workOrder.AssignedTechnicianId,
            Status = workOrder.Status,
            DueDateUtc = workOrder.DueDateUtc,
            Notes = workOrder.Notes,
            CompletedAtUtc = workOrder.CompletedAtUtc,
            CreatedAtUtc = workOrder.CreatedAtUtc,
            UpdatedAtUtc = workOrder.UpdatedAtUtc
        };
    }
}