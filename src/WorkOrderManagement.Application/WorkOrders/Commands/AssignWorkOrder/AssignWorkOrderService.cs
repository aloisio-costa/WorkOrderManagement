using System.Threading;
using WorkOrderManagement.Application.Abstractions.Messaging;
using WorkOrderManagement.Application.Abstractions.Persistence;
using WorkOrderManagement.Application.Common.Results;
using WorkOrderManagement.Application.WorkOrders.Dtos;
using WorkOrderManagement.Application.WorkOrders.Events;
using WorkOrderManagement.Domain.Technicians;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Application.WorkOrders.Commands.AssignWorkOrder;

public class AssignWorkOrderService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly ITechnicianRepository _technicianRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessagePublisher _messagePublisher;

    public AssignWorkOrderService(
        IWorkOrderRepository workOrderRepository,
        ITechnicianRepository technicianRepository,
        IUnitOfWork unitOfWork,
        IMessagePublisher messagePublisher)
    {
        _workOrderRepository = workOrderRepository;
        _technicianRepository = technicianRepository;
        _unitOfWork = unitOfWork;
        _messagePublisher = messagePublisher;
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

        await _messagePublisher.PublishAsync(
            new WorkOrderAssignedEvent(
                workOrder.Id,
                technician.Id,
                DateTime.UtcNow),
            "workorder.assigned");

        return Result<WorkOrderDto>.Success(workOrder.ToDto());
    }
}