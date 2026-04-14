namespace WorkOrderManagement.Application.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}