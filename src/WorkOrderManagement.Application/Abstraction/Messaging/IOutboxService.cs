namespace WorkOrderManagement.Application.Abstractions.Messaging;

public interface IOutboxService
{
    void Add<TMessage>(TMessage message);
}