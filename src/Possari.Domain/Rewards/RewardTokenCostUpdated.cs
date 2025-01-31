using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public record RewardTokenCostUpdated(Guid RewardId, int NewTokenCost, int PreviousTokenCost) : DomainEvent;
