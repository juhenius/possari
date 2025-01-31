using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;
using Possari.Domain.Primitives;

namespace Possari.Application.Rewards.Queries.ListRewards;

public class ListRewardsQueryHandler(IRewardRepository rewardRepository) : IQueryHandler<ListRewardsQuery, List<Reward>>
{
  private readonly IRewardRepository _rewardRepository = rewardRepository;

  public async Task<Result<List<Reward>>> Handle(ListRewardsQuery query, CancellationToken cancellationToken)
  {
    return await _rewardRepository.ListAsync();
  }
}
