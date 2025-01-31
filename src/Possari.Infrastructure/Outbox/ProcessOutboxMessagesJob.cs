using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Primitives;
using Possari.Infrastructure.Common.Persistence;
using Quartz;

namespace Possari.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(PossariDbContext dbContext, IPublisher publisher) : IJob
{
  private readonly PossariDbContext _dbContext = dbContext;
  private readonly IPublisher _publisher = publisher;

  public async Task Execute(IJobExecutionContext context)
  {
    var messages = await _dbContext
      .Set<OutboxMessage>()
      .Where(m => m.ProcessedOnUtc == null)
      .Take(20)
      .ToListAsync(context.CancellationToken);

    foreach (var message in messages)
    {
      var content = JsonConvert.DeserializeObject<DomainEvent>(message.Content,
      new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.All
      });

      if (content is not DomainEvent domainEvent)
      {
        message.ProcessedOnUtc = DateTime.UtcNow;
        message.Error = "NotDomainEvent";
        continue;
      }

      var notification = Activator
        .CreateInstance(typeof(DomainEventNotification<>)
        .MakeGenericType(domainEvent.GetType()), domainEvent);

      if (notification is not INotification)
      {
        message.ProcessedOnUtc = DateTime.UtcNow;
        message.Error = "NotNotification";
        continue;
      }

      await _publisher.Publish(notification, context.CancellationToken);
      message.ProcessedOnUtc = DateTime.UtcNow;
    }

    await _dbContext.SaveChangesAsync();
  }
}
