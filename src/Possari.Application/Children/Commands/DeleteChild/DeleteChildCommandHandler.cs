using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Commands.DeleteChild;

public class DeleteChildCommandHandler(
  IChildRepository childRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<DeleteChildCommand>
{
  private readonly IChildRepository _childRepository = childRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result> Handle(DeleteChildCommand command, CancellationToken cancellationToken)
  {
    var child = await _childRepository.GetByIdAsync(command.Id);

    if (child is null)
    {
      return ChildErrors.NotFound(command.Id);
    }

    var result = child.Delete();

    if (result.IsSuccess)
    {
      await _childRepository.RemoveChildAsync(child);
      await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    return result;
  }
}
