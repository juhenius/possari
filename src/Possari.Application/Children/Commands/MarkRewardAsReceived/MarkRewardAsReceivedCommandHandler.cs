using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Commands.MarkRewardAsReceived;

public class MarkRewardAsReceivedCommandHandler(
  IChildRepository childRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<MarkRewardAsReceivedCommand, Child>
{
  private readonly IChildRepository _childRepository = childRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Child>> Handle(MarkRewardAsReceivedCommand command, CancellationToken cancellationToken)
  {
    var child = await _childRepository.GetByIdAsync(command.Id);

    if (child is null)
    {
      return ChildErrors.NotFound(command.Id);
    }

    var markResult = child.MarkRewardAsReceived(command.PendingRewardId);
    if (markResult.IsFailure)
    {
      return markResult.Error;
    }

    await _childRepository.UpdateChildAsync(child);
    await _unitOfWork.CommitChangesAsync(cancellationToken);

    return child;
  }
}
