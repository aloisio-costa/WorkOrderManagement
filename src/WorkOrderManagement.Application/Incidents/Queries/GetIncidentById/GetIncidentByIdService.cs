using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.Incidents.Dtos;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdService
{
    private readonly IIncidentRepository _incidentRepository;

    public GetIncidentByIdService(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<Result<IncidentDto>> ExecuteAsync(Guid id)
    {
        var incident = await _incidentRepository.GetByIdAsync(id);

        if (incident is null)
        {
            return Result<IncidentDto>.Failure("Incident not found.");
        }

        return Result<IncidentDto>.Success(incident.ToDto());
    }
}