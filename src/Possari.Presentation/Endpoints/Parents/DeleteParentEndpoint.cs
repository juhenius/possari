using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Parents.Commands.DeleteParent;
using Possari.Contracts.Parents;
using Possari.Presentation.Common;

namespace Possari.Presentation.Endpoints.Parents;

public static class DeleteParentEndpoint
{
  public const string Name = "DeleteParent";

  public static IEndpointRouteBuilder MapDeleteParent(this IEndpointRouteBuilder builder)
  {
    builder.MapDelete(ApiEndpoints.Parents.Delete, async (
      Guid parentId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new DeleteParentCommand(parentId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(Results.NoContent);
    })
      .WithName(Name)
      .Produces<ParentResponse>(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
