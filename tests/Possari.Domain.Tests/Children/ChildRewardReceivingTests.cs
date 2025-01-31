using Possari.Domain.Children;
using Possari.Domain.Rewards;

namespace Possari.Domain.Tests.Children;

public class ChildRewardReceivingTests
{
  private static readonly string validName = "name";

  [Fact]
  public void MarkRewardAsReceived_RewardIsNotPending_ReturnsError()
  {
    var child = Child.Create(validName).Value;
    var pendingRewardId = Guid.NewGuid();

    var result = child.MarkRewardAsReceived(pendingRewardId);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.PendingRewardNotFound(pendingRewardId).Code, result.Error.Code);
  }

  [Fact]
  public void MarkRewardAsReceived_PendingRewardExists_Succeeds()
  {
    var tokenCost = 5;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", tokenCost).Value;

    child.AwardTokens(tokenCost);
    child.RedeemReward(reward);

    var pendingRewardId = child.PendingRewards.First().Id;
    var result = child.MarkRewardAsReceived(pendingRewardId);

    Assert.True(result.IsSuccess);
  }

  [Fact]
  public void MarkRewardAsReceived_Succeeds_RemovesPendingReward()
  {
    var tokenCost = 5;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", tokenCost).Value;

    child.AwardTokens(tokenCost);
    child.RedeemReward(reward);

    var pendingRewardId = child.PendingRewards.First().Id;
    var result = child.MarkRewardAsReceived(pendingRewardId);

    Assert.True(result.IsSuccess);
    Assert.DoesNotContain(child.PendingRewards, r => r.Id == pendingRewardId);
  }

  [Fact]
  public void MarkRewardAsReceived_Succeeds_RaisesPendingRewardReceivedDomainEvent()
  {
    var tokenCost = 5;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", tokenCost).Value;

    child.AwardTokens(tokenCost);
    child.RedeemReward(reward);
    child.ClearDomainEvents();

    var pendingRewardId = child.PendingRewards.First().Id;
    child.MarkRewardAsReceived(pendingRewardId);

    Assert.Single(child.DomainEvents);
    Assert.IsType<PendingRewardReceivedDomainEvent>(child.DomainEvents.First());
    Assert.Equal(child.Id, ((PendingRewardReceivedDomainEvent)child.DomainEvents.First()).ChildId);
  }
}
