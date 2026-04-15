using WorkOrderManagement.Application.Abstractions.Persistence;
using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Commands.AssignWorkOrder;

public class AssignWorkOrderService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly ITechnicianRepository _technicianRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AssignWorkOrderService(
        IWorkOrderRepository workOrderRepository,
        ITechnicianRepository technicianRepository,
        IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _technicianRepository = technicianRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WorkOrderDto>> ExecuteAsync(AssignWorkOrderRequest request)
    {
        var workOrder = await _workOrderRepository.GetByIdAsync(request.WorkOrderId);

        if (workOrder is null)
        {
            return Result<WorkOrderDto>.Failure("Work order not found.");
        }

        var technician = await _technicianRepository.GetByIdAsync(request.TechnicianId);

        if (technician is null)
        {
            return Result<WorkOrderDto>.Failure("Technician not found.");
        }

        if (!technician.IsActive)
        {
            return Result<WorkOrderDto>.Failure("Technician is inactive.");
        }

        try
        {
            workOrder.AssignTechnician(request.TechnicianId);
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