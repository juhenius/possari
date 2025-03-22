using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Rewards.Queries.GetReward;
using Possari.Contracts.Rewards;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Rewards;

public static class GetRewardEndpoint
{
  public const string Name = "GetReward";

  public static IEndpointRouteBuilder MapGetReward(this IEndpointRouteBuilder builder)
  {
    builder.MapGet(ApiEndpoints.Rewards.Get, async (
      Guid rewardId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new GetRewardQuery(rewardId);

      var result = await mediator.Send(command, token);

      return result.Match(
        reward => TypedResults.Ok(reward.MapToResponse()),
        (_) => Results.Problem());
    })
      .WithName(Name)
      .Produces<RewardResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
