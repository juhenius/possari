using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;
using Possari.Domain.Primitives;

namespace Possari.Application.Rewards.Commands.DeleteReward;

public class DeleteRewardCommandHandler(
  IRewardRepository rewardRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<DeleteRewardCommand>
{
  private readonly IRewardRepository _rewardRepository = rewardRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result> Handle(DeleteRewardCommand command, CancellationToken cancellationToken)
  {
    var reward = await _rewardRepository.GetByIdAsync(command.Id);

    if (reward is null)
    {
      return RewardErrors.NotFound(command.Id);
    }

    var result = reward.Delete();

    if (result.IsSuccess)
    {
      await _rewardRepository.RemoveRewardAsync(reward);
      await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    return result;
  }
}
