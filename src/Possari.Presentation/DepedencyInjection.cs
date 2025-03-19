using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Possari.Presentation;

public static class DependencyInjection
{
  public static IServiceCollection AddPresentation(this IServiceCollection services)
  {
    var assembly = Assembly.GetExecutingAssembly();

    services.AddControllers().AddApplicationPart(assembly);

    services.AddMediatR(options =>
    {
      options.RegisterServicesFromAssembly(assembly);
    });

    return services;
  }

  public static WebApplication MapPresentation(this WebApplication app)
  {
    app.MapControllers();
    return app;
  }
}
