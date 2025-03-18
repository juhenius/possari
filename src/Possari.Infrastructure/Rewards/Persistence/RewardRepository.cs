using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;
using Possari.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Possari.Infrastructure.Rewards.Persistence;

public class RewardRepository(PossariDbContext dbContext) : IRewardRepository
{
  private readonly PossariDbContext _dbContext = dbContext;

  public async Task AddRewardAsync(Reward reward)
  {
    await _dbContext.Rewards.AddAsync(reward);
  }

  public Task<Reward?> GetByIdAsync(Guid rewardId)
  {
    return _dbContext.Rewards.FirstOrDefaultAsync(a => a.Id == rewardId);
  }

  public async Task<List<Reward>> ListAsync()
  {
    return await _dbContext.Rewards.ToListAsync();
  }

  public Task RemoveRewardAsync(Reward reward)
  {
    _dbContext.Rewards.Remove(reward);

    return Task.CompletedTask;
  }

  public Task UpdateRewardAsync(Reward reward)
  {
    return Task.CompletedTask;
  }
}
