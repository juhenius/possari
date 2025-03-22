using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Commands.CreateChild;
using Possari.Contracts.Children;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Children;

public static class CreateChildEndpoint
{
  public const string Name = "CreateChild";

  public static IEndpointRouteBuilder MapCreateChild(this IEndpointRouteBuilder builder)
  {
    builder.MapPost(ApiEndpoints.Children.Create, async (
      CreateChildRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new CreateChildCommand(request.Name);

      var result = await mediator.Send(command, token);

      return result.Match(
        child => TypedResults.CreatedAtRoute(
          child.MapToResponse(),
          GetChildEndpoint.Name,
          new { ChildId = child.Id }),
        (_) => Results.Problem());
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
