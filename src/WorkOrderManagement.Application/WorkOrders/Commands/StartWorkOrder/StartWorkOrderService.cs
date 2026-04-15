using WorkOrderManagement.Application.Abstractions.Persistence;
using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Commands.StartWorkOrder;

public class StartWorkOrderService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StartWorkOrderService(
        IWorkOrderRepository workOrderRepository,
        IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WorkOrderDto>> ExecuteAsync(Guid workOrderId)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);

        if (workOrder is null)
        {
            return Result<WorkOrderDto>.Failure("Work order not found.");
        }

        try
        {
            workOrder.Start();
        }
        catch (InvalidOperationException ex)
        {
            return Result<WorkOrderDto>.Failure(ex.Message);
        }

        await _workOrderRepository.UpdateAsync(workOrder);
        await _unitOfWork.SaveChangesAsync();

        return Result<WorkOrderDto>.Success(workOrder.ToDto());
    }
}