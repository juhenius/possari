using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public record RewardRenamedDomainEvent(Guid RewardId, string NewName, string PreviousName) : DomainEvent;
