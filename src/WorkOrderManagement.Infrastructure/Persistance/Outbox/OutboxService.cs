using System.Text.Json;
using WorkOrderManagement.Application.Abstractions.Messaging;

namespace WorkOrderManagement.Infrastructure.Persistence.Outbox;

public sealed class OutboxService : IOutboxService
{
    private readonly ApplicationDbContext _dbContext;

    public OutboxService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add<TMessage>(TMessage message)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(TMessage).FullName!,
            Content = JsonSerializer.Serialize(message),
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.OutboxMessages.Add(outboxMessage);
    }
}