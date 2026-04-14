using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Domain.WorkOrders;

namespace WorkOrderManagement.Infrastructure.Persistence.Repositories;

public class WorkOrderRepository : IWorkOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WorkOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(WorkOrder workOrder)
    {
        await _dbContext.WorkOrders.AddAsync(workOrder);
    }

    public async Task<WorkOrder?> GetByIdAsync(Guid id)
    {
        return await _dbContext.WorkOrders.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<WorkOrder>> GetAllAsync()
    {
        return await _dbContext.WorkOrders
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public async Task<bool> ExistsActiveByIncidentIdAsync(Guid incidentId)
    {
        return await _dbContext.WorkOrders.AnyAsync(x =>
            x.IncidentId == incidentId &&
            x.Status != WorkOrderStatus.Completed &&
            x.Status != WorkOrderStatus.Cancelled);
    }

    public Task UpdateAsync(WorkOrder workOrder)
    {
        _dbContext.WorkOrders.Update(workOrder);
        return Task.CompletedTask;
    }
}