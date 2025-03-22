using Possari.Contracts.Children;
using Possari.Domain.Children;

namespace Possari.Presentation.Mapping;

public static class ChildMappingExtensions
{
  public static ChildResponse MapToResponse(this Child child)
  {
    return new ChildResponse(
      child.Id,
      child.Name,
      child.TokenBalance,
      [.. child.PendingRewards.Select(p => new PendingRewardResponse(
        p.Id,
        p.RewardName
      ))]
      );
  }

  public static ChildrenResponse MapToResponse(this IEnumerable<Child> children)
  {
    return new ChildrenResponse(children.Select(MapToResponse));
  }
}
