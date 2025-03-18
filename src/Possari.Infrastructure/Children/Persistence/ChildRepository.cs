using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Possari.Infrastructure.Children.Persistence;

public class ChildRepository(PossariDbContext dbContext) : IChildRepository
{
  private readonly PossariDbContext _dbContext = dbContext;

  public async Task AddChildAsync(Child child)
  {
    await _dbContext.Children.AddAsync(child);
  }

  public Task<Child?> GetByIdAsync(Guid childId)
  {
    return _dbContext.Children
      .Include(c => c.PendingRewards)
      .FirstOrDefaultAsync(c => c.Id == childId);
  }

  public async Task<List<Child>> ListAsync()
  {
    return await _dbContext.Children
      .Include(c => c.PendingRewards)
      .ToListAsync();
  }

  public Task RemoveChildAsync(Child child)
  {
    _dbContext.Children.Remove(child);

    return Task.CompletedTask;
  }

  public Task UpdateChildAsync(Child child)
  {
    return Task.CompletedTask;
  }
}
