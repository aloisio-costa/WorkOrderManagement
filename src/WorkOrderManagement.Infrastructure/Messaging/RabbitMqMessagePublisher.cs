using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WorkOrderManagement.Application.Abstractions.Messaging;

namespace WorkOrderManagement.Infrastructure.Messaging;

public sealed class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<WorkOrderAssignedConsumer> _logger;

    public RabbitMqMessagePublisher(
        IOptions<RabbitMqOptions> options, 
        ILogger<WorkOrderAssignedConsumer> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync<TMessage>(TMessage message, string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Port = _options.Port
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            body: body);

        _logger.LogInformation("Published WorkOrderAssignedEvent message");
    }
}