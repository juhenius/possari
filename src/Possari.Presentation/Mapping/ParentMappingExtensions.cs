using Possari.Contracts.Parents;
using Possari.Domain.Parents;

namespace Possari.Presentation.Mapping;

public static class ParentMappingExtensions
{
  public static ParentResponse MapToResponse(this Parent parent)
  {
    return new ParentResponse(parent.Id, parent.Name);
  }

  public static ParentsResponse MapToResponse(this IEnumerable<Parent> parents)
  {
    return new ParentsResponse(parents.Select(MapToResponse));
  }
}
