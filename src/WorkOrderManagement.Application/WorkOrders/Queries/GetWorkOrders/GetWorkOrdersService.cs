using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersService
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrdersService(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<IReadOnlyList<WorkOrderDto>> ExecuteAsync(GetWorkOrdersRequest request)
    {
        var workOrders = await _workOrderRepository.GetAllAsync();

        var query = workOrders.AsEnumerable();

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.TechnicianId.HasValue)
        {
            query = query.Where(x => x.AssignedTechnicianId == request.TechnicianId.Value);
        }

        if (request.IncidentId.HasValue)
        {
            query = query.Where(x => x.IncidentId == request.IncidentId.Value);
        }

        return query
            .Select(x => x.ToDto())
            .ToList();
    }
}