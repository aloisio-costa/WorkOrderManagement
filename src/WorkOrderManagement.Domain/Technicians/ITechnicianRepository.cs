namespace WorkOrderManagement.Domain.Technicians;

public interface ITechnicianRepository
{
    Task<Technician?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Technician>> GetAllAsync();
}