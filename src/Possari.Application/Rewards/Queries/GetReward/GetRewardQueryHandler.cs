using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;
using Possari.Domain.Primitives;

namespace Possari.Application.Rewards.Queries.GetReward;

public class GetRewardQueryHandler(IRewardRepository rewardRepository) : IQueryHandler<GetRewardQuery, Reward>
{
  private readonly IRewardRepository _rewardRepository = rewardRepository;

  public async Task<Result<Reward>> Handle(GetRewardQuery query, CancellationToken cancellationToken)
  {
    if (await _rewardRepository.GetByIdAsync(query.Id) is not Reward reward)
    {
      return RewardErrors.NotFound(query.Id);
    }

    return reward;
  }
}
