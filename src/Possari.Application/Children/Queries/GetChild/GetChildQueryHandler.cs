using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;
using Possari.Domain.Primitives;

namespace Possari.Application.Children.Queries.GetChild;

public class GetChildQueryHandler(IChildRepository childRepository) : IQueryHandler<GetChildQuery, Child>
{
  private readonly IChildRepository _childRepository = childRepository;

  public async Task<Result<Child>> Handle(GetChildQuery query, CancellationToken cancellationToken)
  {
    if (await _childRepository.GetByIdAsync(query.Id) is not Child child)
    {
      return ChildErrors.NotFound(query.Id);
    }

    return child;
  }
}
