using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid BuildingId { get; init; }
    public string Category { get; init; } = string.Empty;
    public IncidentPriority Priority { get; init; }
    public Guid ReportedByUserId { get; init; }
}