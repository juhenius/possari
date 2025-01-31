using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;
using Possari.Domain.Rewards;

namespace Possari.Application.Children.Commands.RedeemReward;

public class RedeemRewardCommandHandler(
  IChildRepository childRepository,
  IRewardRepository rewardRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<RedeemRewardCommand, Child>
{
  private readonly IChildRepository _childRepository = childRepository;
  private readonly IRewardRepository _rewardRepository = rewardRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Child>> Handle(RedeemRewardCommand command, CancellationToken cancellationToken)
  {
    var child = await _childRepository.GetByIdAsync(command.ChildId);
    if (child is null)
    {
      return ChildErrors.NotFound(command.ChildId);
    }

    var reward = await _rewardRepository.GetByIdAsync(command.RewardId);
    if (reward is null)
    {
      return RewardErrors.NotFound(command.RewardId);
    }

    var redeemRewardResult = child.RedeemReward(reward);
    if (redeemRewardResult.IsFailure)
    {
      return redeemRewardResult.Error;
    }

    await _childRepository.UpdateChildAsync(child);
    await _unitOfWork.CommitChangesAsync(cancellationToken);

    return child;
  }
}
