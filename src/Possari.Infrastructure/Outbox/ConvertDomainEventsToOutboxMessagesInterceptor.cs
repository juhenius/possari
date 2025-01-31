using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Possari.Domain.Primitives;

namespace Possari.Infrastructure.Outbox;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor : SaveChangesInterceptor
{
  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
  DbContextEventData eventData,
  InterceptionResult<int> result,
  CancellationToken cancellationToken = default
)
  {
    DbContext? dbContext = eventData.Context;

    if (dbContext is null)
    {
      return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    var outboxMessages = dbContext.ChangeTracker
      .Entries<AggregateRoot>()
      .Select(x => x.Entity)
      .SelectMany(x =>
      {
        var domainEvents = x.DomainEvents;
        x.ClearDomainEvents();
        return domainEvents;
      })
      .Select(domainEvent => new OutboxMessage
      {
        Id = Guid.NewGuid(),
        OccurredOnUtc = DateTime.UtcNow,
        Type = domainEvent.GetType().Name,
        Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
          TypeNameHandling = TypeNameHandling.All
        }),
      })
      .ToList();

    dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }
}
