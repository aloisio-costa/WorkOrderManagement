namespace WorkOrderManagement.Domain.Incidents;

public interface IIncidentRepository
{
    Task AddAsync(Incident incident);
    Task<Incident?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Incident>> GetAllAsync();
    Task UpdateAsync(Incident incident);
}