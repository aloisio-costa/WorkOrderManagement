using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersRequest
{
    public WorkOrderStatus? Status { get; init; }
    public Guid? TechnicianId { get; init; }
    public Guid? IncidentId { get; init; }
}