using Possari.Domain.Primitives;

namespace Possari.Domain.Parents;

public static class ParentErrors
{
  public static Error NotFound(Guid id) => Error.NotFound("Parents.NotFound", $"Parent with id = '{id}' not found");
  public static Error NameNullOrEmpty => Error.Validation("Parents.NameNullOrEmpty", "Parent name cannot be empty");
}
