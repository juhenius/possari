namespace Possari.Contracts.Children;

public record ChildResponse(Guid Id, string Name, int TokenBalance, List<PendingRewardResponse> PendingRewards);
