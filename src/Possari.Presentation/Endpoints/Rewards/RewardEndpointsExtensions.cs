using Microsoft.AspNetCore.Routing;

namespace Possari.Presentation.Endpoints.Rewards;

public static class RewardEndpointsExtensions
{
  public static IEndpointRouteBuilder MapRewardEndpoints(this IEndpointRouteBuilder builder)
  {
    builder.MapCreateReward();
    builder.MapGetReward();
    builder.MapListRewards();
    builder.MapUpdateReward();
    builder.MapDeleteReward();
    return builder;
  }
}
