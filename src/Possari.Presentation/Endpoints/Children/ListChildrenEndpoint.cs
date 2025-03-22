using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Queries.ListChildren;
using Possari.Contracts.Children;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Children;

public static class ListChildrenEndpoint
{
  public const string Name = "ListChildren";

  public static IEndpointRouteBuilder MapListChildren(this IEndpointRouteBuilder builder)
  {
    builder.MapGet(ApiEndpoints.Children.List, async (
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new ListChildrenQuery();

      var result = await mediator.Send(command, token);

      return result.Match(
        children => TypedResults.Ok(children.MapToResponse()),
        (_) => Results.Problem());
    })
      .WithName(Name)
      .Produces<ChildrenResponse>(StatusCodes.Status200OK);

    return builder;
  }
}
