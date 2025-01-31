using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Queries.ListChildren;

public class ListChildrenQueryHandler(IChildRepository childRepository) : IQueryHandler<ListChildrenQuery, List<Child>>
{
  private readonly IChildRepository _childRepository = childRepository;

  public async Task<Result<List<Child>>> Handle(ListChildrenQuery query, CancellationToken cancellationToken)
  {
    return await _childRepository.ListAsync();
  }
}
