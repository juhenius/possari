using Possari.Application.Tests.Rewards;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children;

public static class TestChildFactory
{
  public static Child CreateChild(string name = "Default Child")
  {
    return Child.Create(name).Value;
  }

  public static List<Child> CreateMultipleChildren(int count)
  {
    return [.. Enumerable.Range(1, count).Select(i => CreateChild($"Child {i}"))];
  }

  public static (Child Child, Guid PendingRewardId) CreateChildWithPendingReward(string name = "Default Child")
  {
    var tokenCost = 2;

    var child = CreateChild(name);
    var reward = TestRewardFactory.CreateReward("test reward", tokenCost);

    child.AwardTokens(tokenCost);
    child.RedeemReward(reward);
    child.ClearDomainEvents();

    return (child, child.PendingRewards.First().Id);
  }
}
