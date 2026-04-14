using Microsoft.EntityFrameworkCore;
using WorkOrderManagement.Domain.Buildings;

namespace WorkOrderManagement.Infrastructure.Persistence.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BuildingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Building?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Buildings.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Building>> GetAllAsync()
    {
        return await _dbContext.Buildings
            .OrderBy(x => x.Name)
            .ToListAsync();
    }
}