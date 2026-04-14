using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Queries.GetIncidents;

public class GetIncidentsRequest
{
    public IncidentStatus? Status { get; init; }
    public IncidentPriority? Priority { get; init; }
    public Guid? BuildingId { get; init; }
}