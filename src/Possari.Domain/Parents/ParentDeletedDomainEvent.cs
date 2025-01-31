using Possari.Domain.Primitives;

namespace Possari.Domain.Parents;

public record ParentDeletedDomainEvent(Guid ParentId) : DomainEvent;
