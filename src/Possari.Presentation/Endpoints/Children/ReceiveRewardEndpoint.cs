using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Commands.MarkRewardAsReceived;
using Possari.Contracts.Children;
using Possari.Presentation.Common;

namespace Possari.Presentation.Endpoints.Children;

public static class ReceiveRewardEndpoint
{
  public const string Name = "ReceiveReward";

  public static IEndpointRouteBuilder MapReceiveReward(this IEndpointRouteBuilder builder)
  {
    builder.MapPatch(ApiEndpoints.Children.ReceiveReward, async (
      Guid childId,
      Guid pendingRewardId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new MarkRewardAsReceivedCommand(childId, pendingRewardId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(Results.NoContent);
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
