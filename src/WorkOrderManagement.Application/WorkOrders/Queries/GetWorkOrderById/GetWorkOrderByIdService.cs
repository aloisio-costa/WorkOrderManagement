using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Queries.GetWorkOrderById;

public class GetWorkOrderByIdService
{
    private readonly IWorkOrderRepository _workOrderRepository;

    public GetWorkOrderByIdService(IWorkOrderRepository workOrderRepository)
    {
        _workOrderRepository = workOrderRepository;
    }

    public async Task<Result<WorkOrderDto>> ExecuteAsync(Guid id)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(id);

        if (workOrder is null)
        {
            return Result<WorkOrderDto>.Failure("Work order not found.");
        }

        return Result<WorkOrderDto>.Success(workOrder.ToDto());
    }
}