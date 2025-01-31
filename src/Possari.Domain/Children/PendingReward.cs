using Possari.Domain.Primitives;
using Possari.Domain.Rewards;

namespace Possari.Domain.Children;

public sealed class PendingReward : Entity
{
  public static Result<PendingReward> Create(Child child, Reward reward)
  {
    return new PendingReward(child.Id, reward.Name);
  }

  public Guid ChildId { get; private set; }
  public string RewardName { get; private set; } = null!;

  private PendingReward(Guid childId, string rewardName, Guid? id = null) : base(id)
  {
    ChildId = childId;
    RewardName = rewardName;
  }

  private PendingReward() { }
}
