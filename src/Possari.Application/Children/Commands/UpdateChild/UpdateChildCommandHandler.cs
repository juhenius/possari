using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Commands.UpdateChild;

public class UpdateChildCommandHandler(
  IChildRepository childRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<UpdateChildCommand, Child>
{
  private readonly IChildRepository _childRepository = childRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Child>> Handle(UpdateChildCommand command, CancellationToken cancellationToken)
  {
    var child = await _childRepository.GetByIdAsync(command.Id);

    if (child is null)
    {
      return ChildErrors.NotFound(command.Id);
    }

    var renameResult = child.Rename(command.Name);
    if (renameResult.IsFailure)
    {
      return renameResult.Error;
    }

    await _childRepository.UpdateChildAsync(child);
    await _unitOfWork.CommitChangesAsync(cancellationToken);

    return child;
  }
}
