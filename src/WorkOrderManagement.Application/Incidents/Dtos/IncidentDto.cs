using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Dtos;

public class IncidentDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid BuildingId { get; init; }
    public string Category { get; init; } = string.Empty;
    public IncidentPriority Priority { get; init; }
    public IncidentStatus Status { get; init; }
    public Guid ReportedByUserId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}