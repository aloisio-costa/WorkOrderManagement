using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Api.Contracts.Incidents;

public class CreateIncidentHttpRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid BuildingId { get; init; }
    public string Category { get; init; } = string.Empty;
    public IncidentPriority Priority { get; init; }
    public Guid ReportedByUserId { get; init; }
}