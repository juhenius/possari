using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Commands.AwardTokens;
using Possari.Contracts.Children;
using Possari.Presentation.Common;

namespace Possari.Presentation.Endpoints.Children;

public static class AwardTokensEndpoint
{
  public const string Name = "AwardTokens";

  public static IEndpointRouteBuilder MapAwardTokens(this IEndpointRouteBuilder builder)
  {
    builder.MapPost(ApiEndpoints.Children.AwardTokens, async (
      Guid childId,
      AwardTokensRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new AwardTokensCommand(childId, request.TokenAmount);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(Results.NoContent);
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
