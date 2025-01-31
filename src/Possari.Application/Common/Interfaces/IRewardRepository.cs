using Possari.Domain.Rewards;

namespace Possari.Application.Common.Interfaces;

public interface IRewardRepository
{
  Task AddRewardAsync(Reward reward);
  Task<Reward?> GetByIdAsync(Guid rewardId);
  Task<List<Reward>> ListAsync();
  Task RemoveRewardAsync(Reward reward);
  Task UpdateRewardAsync(Reward reward);
}
