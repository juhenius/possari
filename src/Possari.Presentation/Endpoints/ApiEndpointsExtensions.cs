using Microsoft.AspNetCore.Routing;
using Possari.Presentation.Endpoints.Children;
using Possari.Presentation.Endpoints.Parents;
using Possari.Presentation.Endpoints.Rewards;

namespace Possari.Presentation.Endpoints;

public static class ApiEndpointsExtensions
{
  public static IEndpointRouteBuilder MapApiEndpoints(this IEndpointRouteBuilder builder)
  {
    builder.MapChildEndpoints();
    builder.MapParentEndpoints();
    builder.MapRewardEndpoints();
    return builder;
  }
}
