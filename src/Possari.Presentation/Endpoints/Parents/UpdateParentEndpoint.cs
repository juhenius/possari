using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Parents.Commands.UpdateParent;
using Possari.Contracts.Parents;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Parents;

public static class UpdateParentEndpoint
{
  public const string Name = "UpdateParent";

  public static IEndpointRouteBuilder MapUpdateParent(this IEndpointRouteBuilder builder)
  {
    builder.MapPatch(ApiEndpoints.Parents.Update, async (
      Guid parentId,
      UpdateParentRequest request,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new UpdateParentCommand(parentId, request.Name);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(p => TypedResults.Ok(p.MapToResponse()));
    })
      .WithName(Name)
      .Produces<ParentResponse>(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
