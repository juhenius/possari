namespace Possari.Domain.Primitives;

public abstract record DomainEvent(
  Guid Id = default,
  DateTime OccurredUtc = default)
{
  public DomainEvent() : this(Guid.NewGuid(), DateTime.UtcNow) { }
}
