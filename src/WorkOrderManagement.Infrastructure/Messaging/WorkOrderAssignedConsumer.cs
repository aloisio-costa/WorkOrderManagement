using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WorkOrderManagement.Application.WorkOrders.Events;

namespace WorkOrderManagement.Infrastructure.Messaging;

public sealed class WorkOrderAssignedConsumer : BackgroundService
{
    private const string QueueName = "workorder.assigned";

    private readonly RabbitMqOptions _options;
    private readonly ILogger<WorkOrderAssignedConsumer> _logger;

    public WorkOrderAssignedConsumer(
        IOptions<RabbitMqOptions> options,
        ILogger<WorkOrderAssignedConsumer> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.HostName,
            UserName = _options.UserName,
            Password = _options.Password,
            Port = _options.Port
        };

        var connection = await factory.CreateConnectionAsync(stoppingToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            try
            {
                _logger.LogInformation("Received WorkOrderAssignedEvent message");

                var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                var message = JsonSerializer.Deserialize<WorkOrderAssignedEvent>(json);

                if (message is null)
                {
                    _logger.LogWarning("Received invalid WorkOrderAssignedEvent message.");
                    return;
                }

                _logger.LogInformation(
                    "Notification simulated: Work order {WorkOrderId} assigned to technician {TechnicianId} at {AssignedAtUtc}",
                    message.WorkOrderId,
                    message.TechnicianId,
                    message.AssignedAtUtc);

                await channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing WorkOrderAssignedEvent.");

                await channel.BasicNackAsync(
                    eventArgs.DeliveryTag,
                    multiple: false,
                    requeue: true);
            }
        };

        await channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);
    }
}