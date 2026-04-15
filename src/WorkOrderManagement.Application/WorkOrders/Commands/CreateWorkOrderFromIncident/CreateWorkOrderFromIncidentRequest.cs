namespace WorkOrderManagement.Application.WorkOrders.Commands.CreateWorkOrderFromIncident;

public class CreateWorkOrderFromIncidentRequest
{
    public Guid IncidentId { get; init; }
    public DateTime? DueDateUtc { get; init; }
    public string? Notes { get; init; }
}