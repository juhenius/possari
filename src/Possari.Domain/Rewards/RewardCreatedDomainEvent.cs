using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public record RewardCreatedDomainEvent(Guid RewardId) : DomainEvent;
