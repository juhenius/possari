using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Parents.Queries.ListParents;
using Possari.Contracts.Parents;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Parents;

public static class ListParentsEndpoint
{
  public const string Name = "ListParents";

  public static IEndpointRouteBuilder MapListParents(this IEndpointRouteBuilder builder)
  {
    builder.MapGet(ApiEndpoints.Parents.List, async (
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new ListParentsQuery();

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(ps => TypedResults.Ok(ps.MapToResponse()));
    })
      .WithName(Name)
      .Produces<ParentsResponse>(StatusCodes.Status200OK);

    return builder;
  }
}
