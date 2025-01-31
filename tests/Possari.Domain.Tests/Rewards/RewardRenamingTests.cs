using Possari.Domain.Rewards;

namespace Possari.Domain.Tests.Rewards;

public class RewardRenamingTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenCost = 1;

  [Fact]
  public void Rename_WithValidName_ChangesName()
  {
    var reward = Reward.Create("initial name", validTokenCost).Value;
    var expectedName = "expected name";

    var result = reward.Rename(expectedName);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, reward.Name);
  }

  [Fact]
  public void Rename_WithEmptyName_Fails()
  {
    var reward = Reward.Create("initial name", validTokenCost).Value;

    var result = reward.Rename("");

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public void Rename_WithChangedName_RaisesRewardRenamedDomainEvent()
  {
    var reward = Reward.Create("initial name", validTokenCost).Value;
    reward.ClearDomainEvents();

    reward.Rename("new name");

    Assert.Single(reward.DomainEvents);
    Assert.IsType<RewardRenamedDomainEvent>(reward.DomainEvents.First());
    Assert.Equal(reward.Id, ((RewardRenamedDomainEvent)reward.DomainEvents.First()).RewardId);
  }

  [Fact]
  public void Rename_WithUnchangedName_DoesNotRaiseDomainEvents()
  {
    var reward = Reward.Create(validName, validTokenCost).Value;
    reward.ClearDomainEvents();

    reward.Rename(validName);

    Assert.Empty(reward.DomainEvents);
  }
}
