using System.Reflection;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Microsoft.EntityFrameworkCore;
using Possari.Infrastructure.Outbox;
using Possari.Domain.Primitives;
using Possari.Domain.Parents;
using Possari.Domain.Rewards;

namespace Possari.Infrastructure.Common.Persistence;

public class PossariDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
  public DbSet<Child> Children { get; set; } = null!;
  public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;
  public DbSet<Parent> Parents { get; set; } = null!;
  public DbSet<Reward> Rewards { get; set; } = null!;

  public async Task CommitChangesAsync(CancellationToken cancellationToken = default)
  {
    await SaveChangesAsync(cancellationToken);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    modelBuilder.Ignore<DomainEvent>();
    base.OnModelCreating(modelBuilder);
  }
}
