using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public record ChildRenamedDomainEvent(Guid ChildId, string NewName, string PreviousName) : DomainEvent;
