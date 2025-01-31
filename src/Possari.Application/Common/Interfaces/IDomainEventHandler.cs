using MediatR;
using Possari.Domain.Primitives;

namespace Possari.Application.Common.Interfaces;

public interface IDomainEventHandler<T> : INotificationHandler<DomainEventNotification<T>> where T : DomainEvent;

public class DomainEventNotification<TDomainEvent>(TDomainEvent domainEvent) : INotification
  where TDomainEvent : DomainEvent
{
  public TDomainEvent DomainEvent { get; } = domainEvent;
}
