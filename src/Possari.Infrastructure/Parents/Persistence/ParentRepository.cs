using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;
using Possari.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Possari.Infrastructure.Parents.Persistence;

public class ParentRepository(PossariDbContext dbContext) : IParentRepository
{
  private readonly PossariDbContext _dbContext = dbContext;

  public async Task AddParentAsync(Parent parent)
  {
    await _dbContext.Parents.AddAsync(parent);
  }

  public Task<Parent?> GetByIdAsync(Guid parentId)
  {
    return _dbContext.Parents.FirstOrDefaultAsync(a => a.Id == parentId);
  }

  public async Task<List<Parent>> ListAsync()
  {
    return await _dbContext.Parents.ToListAsync();
  }

  public Task RemoveParentAsync(Parent parent)
  {
    _dbContext.Parents.Remove(parent);

    return Task.CompletedTask;
  }

  public Task UpdateParentAsync(Parent parent)
  {
    _dbContext.Parents.Update(parent);

    return Task.CompletedTask;
  }
}
