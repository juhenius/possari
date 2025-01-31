using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Commands.CreateChild;

public class CreateChildCommandHandler(
  IChildRepository childRepository,
  IUnitOfWork unitOfWork) : ICommandHandler<CreateChildCommand, Child>
{
  private readonly IChildRepository _childRepository = childRepository;
  private readonly IUnitOfWork _unitOfWork = unitOfWork;

  public async Task<Result<Child>> Handle(CreateChildCommand command, CancellationToken cancellationToken)
  {
    var result = Child.Create(command.Name);

    if (result.IsSuccess)
    {
      await _childRepository.AddChildAsync(result.Value);
      await _unitOfWork.CommitChangesAsync(cancellationToken);
    }

    return result;
  }
}
