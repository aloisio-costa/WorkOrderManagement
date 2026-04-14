using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Domain.Technicians;

namespace WorkOrderManagement.Infrastructure.Persistence.Repositories;

public class TechnicianRepository : ITechnicianRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TechnicianRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Technician?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Technicians.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Technician>> GetAllAsync()
    {
        return await _dbContext.Technicians
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}