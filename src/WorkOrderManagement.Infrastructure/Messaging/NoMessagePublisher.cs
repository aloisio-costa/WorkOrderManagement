using WorkOrderManagement.Application.Abstractions.Messaging;

public sealed class NoOpMessagePublisher : IMessagePublisher
{
    public Task PublishAsync<TMessage>(
        TMessage message,
        string queueName)
    {
        return Task.CompletedTask;
    }
}