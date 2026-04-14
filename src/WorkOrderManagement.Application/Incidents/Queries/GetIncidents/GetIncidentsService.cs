using WorkOrderManagement.Application.Incidents.Dtos;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Queries.GetIncidents;

public class GetIncidentsService
{
    private readonly IIncidentRepository _incidentRepository;

    public GetIncidentsService(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<IReadOnlyList<IncidentDto>> ExecuteAsync(GetIncidentsRequest request)
    {
        var incidents = await _incidentRepository.GetAllAsync();

        var query = incidents.AsEnumerable();

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(x => x.Priority == request.Priority.Value);
        }

        if (request.BuildingId.HasValue)
        {
            query = query.Where(x => x.BuildingId == request.BuildingId.Value);
        }

        return query
            .Select(x => x.ToDto())
            .ToList();
    }
}