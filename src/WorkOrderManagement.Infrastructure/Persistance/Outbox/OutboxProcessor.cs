using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WorkOrderManagement.Application.Abstractions.Messaging;

namespace WorkOrderManagement.Infrastructure.Persistence.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ProcessOutboxMessages();
        }
    }

    private async Task ProcessOutboxMessages()
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

        var messages = await dbContext.OutboxMessages
            .Where(x => x.ProcessedAtUtc == null)
            .OrderBy(x => x.CreatedAtUtc)
            .Take(10)
            .ToListAsync();

        foreach (var message in messages)
        {
            try
            {
                var queueName = GetQueueName(message.Type);

                await publisher.PublishAsync(message.Content, queueName);

                message.ProcessedAtUtc = DateTime.UtcNow;
                message.Error = null;

                _logger.LogInformation(
                    "Outbox message {OutboxMessageId} published successfully",
                    message.Id);
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;

                _logger.LogError(
                    ex,
                    "Error publishing outbox message {OutboxMessageId}",
                    message.Id);
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private static string GetQueueName(string messageType)
    {
        if (messageType.Contains("WorkOrderAssignedEvent"))
        {
            return "workorder.assigned";
        }

        throw new InvalidOperationException($"Unknown outbox message type: {messageType}");
    }
}