using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Possari.Domain.Primitives;

namespace Possari.Infrastructure.Common.Persistence;

public class VersionIncrementInterceptor : SaveChangesInterceptor
{
  public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default
  )
  {
    if (eventData.Context is not null)
    {
      IncrementVersions(eventData.Context);
    }

    return base.SavingChangesAsync(eventData, result, cancellationToken);
  }

  private static void IncrementVersions(DbContext context)
  {
    foreach (var entry in context.ChangeTracker.Entries<Entity>())
    {
      if (entry.State == EntityState.Modified)
      {
        entry.Entity.IncrementVersion();
      }
    }
  }
}
