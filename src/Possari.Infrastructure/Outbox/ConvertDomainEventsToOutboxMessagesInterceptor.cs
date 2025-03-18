using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using Possari.Domain.Primitives;
using Quartz;

namespace Possari.Infrastructure.Outbox;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor(ISchedulerFactory schedulerFactory) : SaveChangesInterceptor
{
  private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> interceptionResult,
    CancellationToken cancellationToken = default
  )
  {
    if (eventData.Context is not null)
    {
      CreateOutboxMessagesFromDomainEvents(eventData.Context);
    }

    return base.SavingChangesAsync(eventData, interceptionResult, cancellationToken);
  }

  public override async ValueTask<int> SavedChangesAsync(
    SaveChangesCompletedEventData eventData,
    int result,
    CancellationToken cancellationToken = default)
  {
    var addedMessages = eventData?.Context?.ChangeTracker.Entries<OutboxMessage>().Count() ?? 0;
    if (addedMessages > 0)
    {
      var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
      var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
      await scheduler.TriggerJob(jobKey, cancellationToken);
    }

    return result;
  }

  private static void CreateOutboxMessagesFromDomainEvents(DbContext context)
  {
    List<OutboxMessage> messages = [.. context.ChangeTracker
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
      })];

    if (messages.Count > 0)
    {
      context.Set<OutboxMessage>().AddRange(messages);
    }
  }
}
