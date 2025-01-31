using Possari.Domain.Rewards;

namespace Possari.Domain.Tests.Rewards;

public class RewardCreationTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenCost = 1;

  [Fact]
  public void Create_WithValidValues_SetsName()
  {
    var expectedName = "expected name";
    var result = Reward.Create(expectedName, validTokenCost);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, result.Value.Name);
  }

  [Fact]
  public void Create_WithValidValues_SetsTokenCost()
  {
    var expectedTokenCost = 5;
    var result = Reward.Create(validName, expectedTokenCost);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedTokenCost, result.Value.TokenCost);
  }

  [Fact]
  public void Create_WithValidValues_UsesRandomId()
  {
    var result = Reward.Create(validName, validTokenCost);

    Assert.True(result.IsSuccess);
    Assert.NotEqual(Guid.Empty, result.Value.Id);
  }

  [Fact]
  public void Create_WithEmptyName_Fails()
  {
    var result = Reward.Create("", validTokenCost);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public void Create_WithTokenCostZero_Fails()
  {
    var result = Reward.Create(validName, 0);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.TokenCostTooLow.Code, result.Error.Code);
  }

  [Fact]
  public void Create_WithValidValues_RaisesRewardCreatedDomainEvent()
  {
    var result = Reward.Create(validName, validTokenCost);

    Assert.True(result.IsSuccess);
    Assert.Single(result.Value.DomainEvents);
    Assert.IsType<RewardCreatedDomainEvent>(result.Value.DomainEvents.First());
    Assert.Equal(result.Value.Id, ((RewardCreatedDomainEvent)result.Value.DomainEvents.First()).RewardId);
  }
}
