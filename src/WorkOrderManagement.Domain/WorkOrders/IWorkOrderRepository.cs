namespace WorkOrderManagement.Domain.WorkOrders;

public interface IWorkOrderRepository
{
    Task AddAsync(WorkOrder workOrder);
    Task<WorkOrder?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<WorkOrder>> GetAllAsync();
    Task<bool> ExistsActiveByIncidentIdAsync(Guid incidentId);
    Task UpdateAsync(WorkOrder workOrder);
}