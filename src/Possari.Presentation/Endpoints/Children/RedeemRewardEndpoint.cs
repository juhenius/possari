using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Commands.RedeemReward;
using Possari.Contracts.Children;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Children;

public static class RedeemRewardEndpoint
{
  public const string Name = "RedeemReward";

  public static IEndpointRouteBuilder MapRedeemReward(this IEndpointRouteBuilder builder)
  {
    builder.MapPost(ApiEndpoints.Children.RedeemReward, async (
      Guid childId,
      RedeemRewardRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new RedeemRewardCommand(childId, request.RewardId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(Results.NoContent);
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status409Conflict)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
