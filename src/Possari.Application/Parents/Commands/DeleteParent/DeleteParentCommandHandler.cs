using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;
using Possari.Domain.Primitives;

namespace Possari.Application.Parents.Commands.DeleteParent;

public class DeleteParentCommandHandler(
  IParentRepository parentRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<DeleteParentCommand>
{
  private readonly IParentRepository _parentRepository = parentRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result> Handle(DeleteParentCommand command, CancellationToken cancellationToken)
  {
    var parent = await _parentRepository.GetByIdAsync(command.Id);

    if (parent is null)
    {
      return ParentErrors.NotFound(command.Id);
    }

    var result = parent.Delete();

    if (result.IsSuccess)
    {
      await _parentRepository.RemoveParentAsync(parent);
      await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    return result;
  }
}
