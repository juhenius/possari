using Possari.Domain.Children;
using Possari.Domain.Rewards;

namespace Possari.Domain.Tests.Children;

public class ChildRewardRedeemingTests
{
  private static readonly string validName = "name";

  [Fact]
  public void RedeemReward_WithInsufficientTokenBalance_FailsToRedeem()
  {
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", 100).Value;

    var result = child.RedeemReward(reward);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.InsufficientTokenBalance.Code, result.Error.Code);
  }

  [Fact]
  public void RedeemReward_WithEnoughTokenBalance_Succeeds()
  {
    var tokenBalance = 5;
    var rewardTokenCost = 1;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", rewardTokenCost).Value;

    child.AwardTokens(tokenBalance);

    var result = child.RedeemReward(reward);

    Assert.True(result.IsSuccess);
  }

  [Fact]
  public void RedeemReward_WithExactTokenBalance_Succeeds()
  {
    var tokenBalance = 5;
    var rewardTokenCost = 5;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", rewardTokenCost).Value;

    child.AwardTokens(tokenBalance);

    var result = child.RedeemReward(reward);

    Assert.True(result.IsSuccess);
  }

  [Fact]
  public void RedeemReward_Succeeds_SubtractsTokenBalance()
  {
    var tokenBalance = 5;
    var rewardTokenCost = 2;
    var expectedTokenBalanceAfterRedeem = 3;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", rewardTokenCost).Value;

    child.AwardTokens(tokenBalance);

    var result = child.RedeemReward(reward);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedTokenBalanceAfterRedeem, child.TokenBalance);
  }

  [Fact]
  public void RedeemReward_Succeeds_AddsToPendingRewards()
  {
    var tokenBalance = 5;
    var rewardTokenCost = 1;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", rewardTokenCost).Value;

    child.AwardTokens(tokenBalance);
    child.RedeemReward(reward);

    Assert.Contains(child.PendingRewards, r => r.RewardName == reward.Name);
  }

  [Fact]
  public void RedeemReward_Succeeds_RaisesRewardRedeemedDomainEvent()
  {
    var tokenBalance = 5;
    var rewardTokenCost = 1;
    var child = Child.Create(validName).Value;
    var reward = Reward.Create("test reward", rewardTokenCost).Value;

    child.AwardTokens(tokenBalance);
    child.ClearDomainEvents();

    child.RedeemReward(reward);

    Assert.Single(child.DomainEvents);
    Assert.IsType<RewardRedeemedDomainEvent>(child.DomainEvents.First());
    Assert.Equal(child.Id, ((RewardRedeemedDomainEvent)child.DomainEvents.First()).ChildId);
  }
}
