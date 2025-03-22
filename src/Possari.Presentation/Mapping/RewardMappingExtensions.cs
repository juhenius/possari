using Possari.Contracts.Rewards;
using Possari.Domain.Rewards;

namespace Possari.Presentation.Mapping;

public static class RewardMappingExtensions
{
  public static RewardResponse MapToResponse(this Reward reward)
  {
    return new RewardResponse(reward.Id, reward.Name, reward.TokenCost);
  }

  public static RewardsResponse MapToResponse(this IEnumerable<Reward> rewards)
  {
    return new RewardsResponse(rewards.Select(MapToResponse));
  }
}
