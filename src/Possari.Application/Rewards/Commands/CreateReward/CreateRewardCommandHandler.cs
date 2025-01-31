using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;
using Possari.Domain.Primitives;

namespace Possari.Application.Rewards.Commands.CreateReward;

public class CreateRewardCommandHandler(
  IRewardRepository rewardRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<CreateRewardCommand, Reward>
{
  private readonly IRewardRepository _rewardRepository = rewardRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Reward>> Handle(CreateRewardCommand command, CancellationToken cancellationToken)
  {
    var result = Reward.Create(command.Name, command.TokenCost);

    if (result.IsSuccess)
    {
      await _rewardRepository.AddRewardAsync(result.Value);
      await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    return result;
  }
}
