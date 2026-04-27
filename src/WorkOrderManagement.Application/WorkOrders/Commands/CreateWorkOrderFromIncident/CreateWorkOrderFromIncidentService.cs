using Microsoft.Extensions.Caching.Memory;
using WorkOrderManagement.Application.Abstractions.Persistence;
using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Domain.Incidents;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Commands.CreateWorkOrderFromIncident;

public class CreateWorkOrderFromIncidentService
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;

    public CreateWorkOrderFromIncidentService(
        IIncidentRepository incidentRepository,
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork,
         IMemoryCache cache)
    {
        _incidentRepository = incidentRepository;
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result<WorkOrderDto>> ExecuteAsync(CreateWorkOrderFromIncidentRequest request)
    {
        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);

        if (incident is null)
        {
            return Result<WorkOrderDto>.Failure("Incident not found.");
        }

        var hasActiveWorkOrder = await _workOrderRepository.ExistsActiveByIncidentIdAsync(request.IncidentId);

        if (hasActiveWorkOrder)
        {
            return Result<WorkOrderDto>.Failure("An active work order already exists for this incident.");
        }

        var workOrder = new WorkOrder(
            request.IncidentId,
            request.DueDateUtc,
            request.Notes);

        await _workOrderRepository.AddAsync(workOrder);
        await _unitOfWork.SaveChangesAsync();

        _cache.Remove("workorders");

        return Result<WorkOrderDto>.Success(workOrder.ToDto());
    }
}