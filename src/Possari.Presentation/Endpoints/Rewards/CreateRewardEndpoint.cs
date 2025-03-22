using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Rewards.Commands.CreateReward;
using Possari.Contracts.Rewards;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Rewards;

public static class CreateRewardEndpoint
{
  public const string Name = "CreateReward";

  public static IEndpointRouteBuilder MapCreateReward(this IEndpointRouteBuilder builder)
  {
    builder.MapPost(ApiEndpoints.Rewards.Create, async (
      CreateRewardRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new CreateRewardCommand(request.Name, request.TokenCost);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(
        r => TypedResults.CreatedAtRoute(
          r.MapToResponse(),
          GetRewardEndpoint.Name,
          new { RewardId = r.Id }));
    })
      .WithName(Name)
      .Produces<RewardResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
