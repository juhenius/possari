using Possari.Domain.Rewards;

namespace Possari.Domain.Tests.Rewards;

public class RewardDeletionTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenCost = 1;

  [Fact]
  public void Delete_RaisesRewardDeletedDomainEvent()
  {
    var reward = Reward.Create(validName, validTokenCost).Value;
    reward.ClearDomainEvents();

    reward.Delete();

    Assert.Single(reward.DomainEvents);
    Assert.IsType<RewardDeletedDomainEvent>(reward.DomainEvents.First());
    Assert.Equal(reward.Id, ((RewardDeletedDomainEvent)reward.DomainEvents.First()).RewardId);
  }
}
