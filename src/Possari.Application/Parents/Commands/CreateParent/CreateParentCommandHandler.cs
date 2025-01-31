using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;
using Possari.Domain.Primitives;

namespace Possari.Application.Parents.Commands.CreateParent;

public class CreateParentCommandHandler(
  IParentRepository parentRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<CreateParentCommand, Parent>
{
  private readonly IParentRepository _parentRepository = parentRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Parent>> Handle(CreateParentCommand command, CancellationToken cancellationToken)
  {
    var result = Parent.Create(command.Name);

    if (result.IsSuccess)
    {
      await _parentRepository.AddParentAsync(result.Value);
      await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    return result;
  }
}
