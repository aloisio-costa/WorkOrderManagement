namespace WorkOrderManagement.Domain.Buildings;

public interface IBuildingRepository
{
    Task<Building?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Building>> GetAllAsync();
}