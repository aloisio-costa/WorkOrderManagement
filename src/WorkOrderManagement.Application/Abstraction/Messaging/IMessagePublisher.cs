namespace WorkOrderManagement.Application.Abstractions.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<TMessage>(TMessage message, string queueName);
}