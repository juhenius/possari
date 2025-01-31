namespace Possari.Domain.Primitives;

public abstract class AggregateRoot : Entity
{
  protected AggregateRoot(Guid? id) : base(id)
  {
  }

  protected AggregateRoot()
  {
  }

  private readonly List<DomainEvent> _domainEvents = [];

  public IReadOnlyCollection<DomainEvent> DomainEvents => [.. _domainEvents];

  public void ClearDomainEvents() => _domainEvents.Clear();

  protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
