namespace WorkOrderManagement.Api.Contracts.WorkOrders;

public class AssignWorkOrderHttpRequest
{
    public Guid TechnicianId { get; init; }
}