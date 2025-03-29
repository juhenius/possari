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

      return result.ToHttpResult(
        p => TypedResults.CreatedAtRoute(
          p.MapToResponse(),
          GetParentEndpoint.Name,
          new { ParentId = p.Id }));
    })
      .WithName(Name)
      .Produces<ParentResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status400BadRequest)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
