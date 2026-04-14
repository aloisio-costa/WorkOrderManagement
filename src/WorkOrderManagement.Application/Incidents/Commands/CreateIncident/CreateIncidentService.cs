using WorkOrderManagement.Application.Abstractions.Persistence;
using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.Incidents.Dtos;
using WorkOrderManagement.Domain.Buildings;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IBuildingRepository _buildingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateIncidentService(
        IIncidentRepository incidentRepository,
        IBuildingRepository buildingRepository,
        IUnitOfWork unitOfWork)
    {
        _incidentRepository = incidentRepository;
        _buildingRepository = buildingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IncidentDto>> ExecuteAsync(CreateIncidentRequest request)
    {
        var building = await _buildingRepository.GetByIdAsync(request.BuildingId);

        if (building is null)
        {
            return Result<IncidentDto>.Failure("Building not found.");
        }

        var incident = new Incident(
            request.Title,
            request.Description,
            request.BuildingId,
            request.Category,
            request.Priority,
            request.ReportedByUserId);

        await _incidentRepository.AddAsync(incident);
        await _unitOfWork.SaveChangesAsync();

        return Result<IncidentDto>.Success(incident.ToDto());
    }
}