using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Commands.UpdateChild;
using Possari.Contracts.Children;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Children;

public static class UpdateChildEndpoint
{
  public const string Name = "UpdateChild";

  public static IEndpointRouteBuilder MapUpdateChild(this IEndpointRouteBuilder builder)
  {
    builder.MapPatch(ApiEndpoints.Children.Update, async (
      Guid childId,
      UpdateChildRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new UpdateChildCommand(childId, request.Name);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(c => TypedResults.Ok(c.MapToResponse()));
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status400BadRequest)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
