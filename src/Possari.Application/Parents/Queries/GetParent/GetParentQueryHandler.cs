using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;
using Possari.Domain.Primitives;

namespace Possari.Application.Parents.Queries.GetParent;

public class GetParentQueryHandler(IParentRepository parentRepository) : IQueryHandler<GetParentQuery, Parent>
{
  private readonly IParentRepository _parentRepository = parentRepository;

  public async Task<Result<Parent>> Handle(GetParentQuery query, CancellationToken cancellationToken)
  {
    if (await _parentRepository.GetByIdAsync(query.Id) is not Parent parent)
    {
      return ParentErrors.NotFound(query.Id);
    }

    return parent;
  }
}
