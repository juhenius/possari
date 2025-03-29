using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Parents.Queries.GetParent;
using Possari.Contracts.Parents;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Parents;

public static class GetParentEndpoint
{
  public const string Name = "GetParent";

  public static IEndpointRouteBuilder MapGetParent(this IEndpointRouteBuilder builder)
  {
    builder.MapGet(ApiEndpoints.Parents.Get, async (
      Guid parentId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new GetParentQuery(parentId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(p => TypedResults.Ok(p.MapToResponse()));
    })
      .WithName(Name)
      .Produces<ParentResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
