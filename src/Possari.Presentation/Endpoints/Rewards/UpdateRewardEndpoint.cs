using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Rewards.Commands.UpdateReward;
using Possari.Contracts.Rewards;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Rewards;

public static class UpdateRewardEndpoint
{
  public const string Name = "UpdateReward";

  public static IEndpointRouteBuilder MapUpdateReward(this IEndpointRouteBuilder builder)
  {
    builder.MapPatch(ApiEndpoints.Rewards.Update, async (
      Guid rewardId,
      UpdateRewardRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new UpdateRewardCommand(rewardId, request.Name, request.TokenCost);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(r => TypedResults.Ok(r.MapToResponse()));
    })
      .WithName(Name)
      .Produces<RewardResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
