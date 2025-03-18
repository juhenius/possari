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
    if (eventData.Context is not null)
    {
      CreateOutboxMessagesFromDomainEvents(eventData.Context);
    }

    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private static void CreateOutboxMessagesFromDomainEvents(DbContext context)
  {
    context.Set<OutboxMessage>().AddRange(context.ChangeTracker
      .Entries<AggregateRoot>()
      .SelectMany(x => x.Entity.PopDomainEvents())
      .Select(domainEvent => new OutboxMessage
      {
        Id = Guid.NewGuid(),
        OccurredOnUtc = DateTime.UtcNow,
        Type = domainEvent.GetType().Name,
        Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
          TypeNameHandling = TypeNameHandling.All
        }),
      }).ToList());
  }
}
