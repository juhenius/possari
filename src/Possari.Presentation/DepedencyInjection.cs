using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Possari.Presentation.Endpoints;

namespace Possari.Presentation;

public static class DependencyInjection
{
  public static IServiceCollection AddPresentation(this IServiceCollection services)
  {
    var assembly = Assembly.GetExecutingAssembly();

    services.AddMediatR(options =>
    {
      options.RegisterServicesFromAssembly(assembly);
    });

    return services;
  }

  public static WebApplication MapPresentation(this WebApplication app)
  {
    app.MapApiEndpoints();
    return app;
  }
}
