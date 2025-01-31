using Possari.Domain.Rewards;

namespace Possari.Domain.Tests.Rewards;

public class RewardTokenCostUpdatingTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenCost = 1;

  [Fact]
  public void UpdateTokenCost_WithValidTokenCost_ChangesTokenCost()
  {
    var reward = Reward.Create(validName, 1).Value;
    var expectedTokenCost = 5;

    var result = reward.UpdateTokenCost(expectedTokenCost);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedTokenCost, reward.TokenCost);
  }

  [Fact]
  public void UpdateTokenCost_WithTokenCostZero_Fails()
  {
    var reward = Reward.Create(validName, 1).Value;

    var result = reward.UpdateTokenCost(0);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.TokenCostTooLow.Code, result.Error.Code);
  }

  [Fact]
  public void UpdateTokenCost_WithChangedTokenCost_RaisesRewardTokenCostUpdated()
  {
    var reward = Reward.Create(validName, 1).Value;
    reward.ClearDomainEvents();

    reward.UpdateTokenCost(5);

    Assert.Single(reward.DomainEvents);
    Assert.IsType<RewardTokenCostUpdated>(reward.DomainEvents.First());
    Assert.Equal(reward.Id, ((RewardTokenCostUpdated)reward.DomainEvents.First()).RewardId);
  }

  [Fact]
  public void UpdateTokenCost_WithUnchangedTokenCost_DoesNotRaiseDomainEvents()
  {
    var reward = Reward.Create(validName, validTokenCost).Value;
    reward.ClearDomainEvents();

    reward.UpdateTokenCost(validTokenCost);

    Assert.Empty(reward.DomainEvents);
  }
}
