using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;
using Possari.Domain.Primitives;

namespace Possari.Application.Rewards.Commands.UpdateReward;

public class UpdateRewardCommandHandler(
  IRewardRepository rewardRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<UpdateRewardCommand, Reward>
{
  private readonly IRewardRepository _rewardRepository = rewardRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Reward>> Handle(UpdateRewardCommand command, CancellationToken cancellationToken)
  {
    var reward = await _rewardRepository.GetByIdAsync(command.Id);

    if (reward is null)
    {
      return RewardErrors.NotFound(command.Id);
    }

    var renameResult = reward.Rename(command.Name);
    if (renameResult.IsFailure)
    {
      return renameResult.Error;
    }

    var updateTokenCostResult = reward.UpdateTokenCost(command.TokenCost);
    if (updateTokenCostResult.IsFailure)
    {
      return updateTokenCostResult.Error;
    }

    await _rewardRepository.UpdateRewardAsync(reward);
    await _unitOfWork.CommitChangesAsync(cancellationToken);

    return reward;
  }
}
