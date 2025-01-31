using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public record RewardDeletedDomainEvent(Guid RewardId) : DomainEvent;
