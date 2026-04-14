using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Dtos;

public static class IncidentMappings
{
    public static IncidentDto ToDto(this Incident incident)
    {
        return new IncidentDto
        {
            Id = incident.Id,
            Title = incident.Title,
            Description = incident.Description,
            BuildingId = incident.BuildingId,
            Category = incident.Category,
            Priority = incident.Priority,
            Status = incident.Status,
            ReportedByUserId = incident.ReportedByUserId,
            CreatedAtUtc = incident.CreatedAtUtc,
            UpdatedAtUtc = incident.UpdatedAtUtc
        };
    }
}