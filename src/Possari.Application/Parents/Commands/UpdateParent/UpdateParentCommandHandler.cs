using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;
using Possari.Domain.Primitives;

namespace Possari.Application.Parents.Commands.UpdateParent;

public class UpdateParentCommandHandler(
  IParentRepository parentRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<UpdateParentCommand, Parent>
{
  private readonly IParentRepository _parentRepository = parentRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Parent>> Handle(UpdateParentCommand command, CancellationToken cancellationToken)
  {
    var parent = await _parentRepository.GetByIdAsync(command.Id);

    if (parent is null)
    {
      return ParentErrors.NotFound(command.Id);
    }

    var renameResult = parent.Rename(command.Name);
    if (renameResult.IsFailure)
    {
      return renameResult.Error;
    }

    await _parentRepository.UpdateParentAsync(parent);
    await _unitOfWork.CommitChangesAsync(cancellationToken);

    return parent;
  }
}
