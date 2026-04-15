namespace WorkOrderManagement.Api.Contracts.WorkOrders;

public class CreateWorkOrderFromIncidentHttpRequest
{
    public DateTime? DueDateUtc { get; init; }
    public string? Notes { get; init; }
}