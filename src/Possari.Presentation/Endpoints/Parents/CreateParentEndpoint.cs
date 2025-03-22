using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Parents.Commands.CreateParent;
using Possari.Contracts.Parents;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Parents;

public static class CreateParentEndpoint
{
  public const string Name = "CreateParent";

  public static IEndpointRouteBuilder MapCreateParent(this IEndpointRouteBuilder builder)
  {
    builder.MapPost(ApiEndpoints.Parents.Create, async (
      CreateParentRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new CreateParentCommand(request.Name);

      var result = await mediator.Send(command, token);

      return result.Match(
        parent => TypedResults.CreatedAtRoute(
          parent.MapToResponse(),
          GetParentEndpoint.Name,
          new { ParentId = parent.Id }),
        (_) => Results.Problem());
    })
      .WithName(Name)
      .Produces<ParentResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
