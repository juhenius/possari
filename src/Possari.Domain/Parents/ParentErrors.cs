using Possari.Domain.Primitives;

namespace Possari.Domain.Parents;

public static class ParentErrors
{
  public static Error NotFound(Guid id) => new("Parents.NotFound", $"Parent with id = '{id}' not found");
  public static Error NameNullOrEmpty => new("Parents.NameNullOrEmpty", "Parent name cannot be empty");
}
