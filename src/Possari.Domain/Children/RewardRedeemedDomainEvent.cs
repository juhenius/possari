using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public record RewardRedeemedDomainEvent(Guid ChildId, Guid RewardId) : DomainEvent;
