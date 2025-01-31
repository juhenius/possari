using Possari.Domain.Rewards;

namespace Possari.Application.Tests.Rewards;

public static class TestRewardFactory
{
  public static Reward CreateReward(
      string name = "Default Reward",
      int tokenCost = 10)
  {
    return Reward.Create(name, tokenCost).Value;
  }

  public static List<Reward> CreateMultipleRewards(int count)
  {
    return [.. Enumerable.Range(1, count).Select(i => CreateReward($"Reward {i}", i * 10))];
  }
}
