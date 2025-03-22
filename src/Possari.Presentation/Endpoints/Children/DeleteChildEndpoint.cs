using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Children.Commands.DeleteChild;
using Possari.Contracts.Children;
using Possari.Presentation.Common;

namespace Possari.Presentation.Endpoints.Children;

public static class DeleteChildEndpoint
{
  public const string Name = "DeleteChild";

  public static IEndpointRouteBuilder MapDeleteChild(this IEndpointRouteBuilder builder)
  {
    builder.MapDelete(ApiEndpoints.Children.Delete, async (
      Guid childId,
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new DeleteChildCommand(childId);

      var result = await mediator.Send(command, token);

      return result.ToHttpResult(Results.NoContent);
    })
      .WithName(Name)
      .Produces<ChildResponse>(StatusCodes.Status204NoContent)
      .Produces(StatusCodes.Status500InternalServerError);

    return builder;
  }
}
