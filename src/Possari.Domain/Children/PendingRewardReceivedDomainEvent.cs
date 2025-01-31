using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public record PendingRewardReceivedDomainEvent(Guid ChildId, string RewardName) : DomainEvent;
