using Possari.Domain.Primitives;

namespace Possari.Domain.Parents;

public record ParentCreatedDomainEvent(Guid ParentId) : DomainEvent;
