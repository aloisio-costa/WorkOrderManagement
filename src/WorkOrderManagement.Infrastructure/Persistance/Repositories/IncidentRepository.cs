using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Domain.Incidents;

namespace WorkOrderManagement.Infrastructure.Persistence.Repositories;

public class IncidentRepository : IIncidentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public IncidentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Incident incident)
    {
        await _dbContext.Incidents.AddAsync(incident);
    }

    public async Task<Incident?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Incidents.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Incident>> GetAllAsync()
    {
        return await _dbContext.Incidents
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync();
    }

    public Task UpdateAsync(Incident incident)
    {
        _dbContext.Incidents.Update(incident);
        return Task.CompletedTask;
    }
}