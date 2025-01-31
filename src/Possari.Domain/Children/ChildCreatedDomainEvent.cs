using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public record ChildCreatedDomainEvent(Guid ChildId) : DomainEvent;
