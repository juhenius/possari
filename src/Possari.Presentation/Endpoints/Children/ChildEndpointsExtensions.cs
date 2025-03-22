using Microsoft.AspNetCore.Routing;

namespace Possari.Presentation.Endpoints.Children;

public static class ChildEndpointsExtensions
{
  public static IEndpointRouteBuilder MapChildEndpoints(this IEndpointRouteBuilder builder)
  {
    builder.MapCreateChild();
    builder.MapGetChild();
    builder.MapListChildren();
    builder.MapUpdateChild();
    builder.MapDeleteChild();
    builder.MapAwardTokens();
    builder.MapRedeemReward();
    builder.MapReceiveReward();
    return builder;
  }
}
