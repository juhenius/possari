using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public record TokensAwardedDomainEvent(Guid ChildId, int TokenAmount) : DomainEvent;
