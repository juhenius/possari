using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Commands.AwardTokens;

public class AwardTokensCommandHandler(
  IChildRepository childRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<AwardTokensCommand, Child>
{
  private readonly IChildRepository _childRepository = childRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Child>> Handle(AwardTokensCommand command, CancellationToken cancellationToken)
  {
    var child = await _childRepository.GetByIdAsync(command.Id);

    if (child is null)
    {
      return ChildErrors.NotFound(command.Id);
    }

    var awardTokenResult = child.AwardTokens(command.TokenAmount);
    if (awardTokenResult.IsFailure)
    {
      return awardTokenResult.Error;
    }

    await _childRepository.UpdateChildAsync(child);
    await _unitOfWork.CommitChangesAsync(cancellationToken);

    return child;
  }
}
