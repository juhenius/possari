using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Rewards.Commands.DeleteReward;
using Possari.Contracts.Rewards;
using Possari.Presentation.Common;

namespace Possari.Presentation.Endpoints.Rewards;

public static class DeleteRewardEndpoint
{
  public const string Name = "DeleteReward";

  public static IEndpointRouteBuilder MapDeleteReward(this IEndpointRouteBuilder builder)
  {
    builder.MapDelete(ApiEndpoints.Rewards.Delete, async (
      Guid rewardId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new DeleteRewardCommand(rewardId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(Results.NoContent);
    })
      .WithName(Name)
      .Produces<RewardResponse>(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
