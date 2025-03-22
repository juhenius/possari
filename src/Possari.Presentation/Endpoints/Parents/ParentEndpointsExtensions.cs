using Microsoft.AspNetCore.Routing;

namespace Possari.Presentation.Endpoints.Parents;

public static class ParentEndpointsExtensions
{
  public static IEndpointRouteBuilder MapParentEndpoints(this IEndpointRouteBuilder builder)
  {
    builder.MapCreateParent();
    builder.MapGetParent();
    builder.MapListParents();
    builder.MapUpdateParent();
    builder.MapDeleteParent();
    return builder;
  }
}
