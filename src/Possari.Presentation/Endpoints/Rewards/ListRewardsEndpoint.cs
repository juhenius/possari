using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Possari.Application.Rewards.Queries.ListRewards;
using Possari.Contracts.Rewards;
using Possari.Presentation.Common;
using Possari.Presentation.Mapping;

namespace Possari.Presentation.Endpoints.Rewards;

public static class ListRewardsEndpoint
{
  public const string Name = "ListRewards";

  public static IEndpointRouteBuilder MapListRewards(this IEndpointRouteBuilder builder)
  {
    builder.MapGet(ApiEndpoints.Rewards.List, async (
      ISender mediator,
      CancellationToken token) =>
    {
      var command = new ListRewardsQuery();

      var result = await mediator.Send(command, token);

      return result.Match(
        rewards => TypedResults.Ok(rewards.MapToResponse()),
        (_) => Results.Problem());
    })
      .WithName(Name)
      .Produces<RewardsResponse>(StatusCodes.Status200OK);

    return builder;
  }
}
