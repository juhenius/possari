using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;
using Possari.Domain.Primitives;

namespace Possari.Application.Parents.Queries.ListParents;

public class ListParentsQueryHandler(IParentRepository parentRepository) : IQueryHandler<ListParentsQuery, List<Parent>>
{
  private readonly IParentRepository _parentRepository = parentRepository;

  public async Task<Result<List<Parent>>> Handle(ListParentsQuery query, CancellationToken cancellationToken)
  {
    return await _parentRepository.ListAsync();
  }
}
