using Possari.Application.Common.Interfaces;
using Possari.Infrastructure.Children.Persistence;
using Possari.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Possari.Infrastructure.Outbox;
using Quartz;
using Possari.Infrastructure.Parents.Persistence;
using Possari.Infrastructure.Rewards.Persistence;

namespace Possari.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services)
  {
    return services
    .AddPersistence();
  }

  public static IServiceCollection AddPersistence(this IServiceCollection services)
  {
    services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();
    services.AddDbContext<PossariDbContext>((sp, op) =>
    {
      var interceptor = sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();
      op.UseSqlite("Data Source = Possari.db")
        .AddInterceptors(interceptor);
    });

    services.AddScoped<IChildRepository, ChildRepository>();
    services.AddScoped<IParentRepository, ParentRepository>();
    services.AddScoped<IRewardRepository, RewardRepository>();
    services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<PossariDbContext>());

    services.AddQuartz(configure =>
    {
      var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
      configure.AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(trigger =>
        {
          trigger.ForJob(jobKey)
            .WithSimpleSchedule(s => s.WithIntervalInSeconds(10).RepeatForever());
        });
    });

    services.AddQuartzHostedService(options =>
    {
      options.WaitForJobsToComplete = true;
    });

    return services;
  }
}
