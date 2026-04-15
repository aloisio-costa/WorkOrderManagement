namespace WorkOrderManagement.Application.WorkOrders.Commands.AssignWorkOrder;

public class AssignWorkOrderRequest
{
    public Guid WorkOrderId { get; init; }
    public Guid TechnicianId { get; init; }
}