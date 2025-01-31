using Microsoft.Extensions.DependencyInjection;

namespace Possari.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    services.AddMediatR(options =>
    {
      options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
    });

    return services;
  }
}
