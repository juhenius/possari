using Possari.Domain.Primitives;

namespace Possari.Domain.Parents;

public record ParentRenamedDomainEvent(Guid ParentId, string NewName, string PreviousName) : DomainEvent;
