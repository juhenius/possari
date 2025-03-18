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

  public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

  public void ClearDomainEvents() => _domainEvents.Clear();

  public IReadOnlyCollection<DomainEvent> PopDomainEvents()
  {
    List<DomainEvent> result = [.. _domainEvents];
    _domainEvents.Clear();
    return result.AsReadOnly();
  }

  protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
