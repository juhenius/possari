using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public record ChildDeletedDomainEvent(Guid ChildId) : DomainEvent;
