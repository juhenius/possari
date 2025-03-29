using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Queries.GetChild;
using Possari.Contracts.Children;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Children;

public static class GetChildEndpoint
{
  public const string Name = "GetChild";

  public static IEndpointRouteBuilder MapGetChild(this IEndpointRouteBuilder builder)
  {
    builder.MapGet(ApiEndpoints.Children.Get, async (
      Guid childId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new GetChildQuery(childId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(c => TypedResults.Ok(c.MapToResponse()));
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status404NotFound)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
