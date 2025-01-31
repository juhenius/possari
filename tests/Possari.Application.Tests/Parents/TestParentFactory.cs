using Possari.Domain.Parents;

namespace Possari.Application.Tests.Parents;

public static class TestParentFactory
{
  public static Parent CreateParent(string name = "Default Parent")
  {
    return Parent.Create(name).Value;
  }

  public static List<Parent> CreateMultipleParents(int count)
  {
    return [.. Enumerable.Range(1, count).Select(i => CreateParent($"Parent {i}"))];
  }
}
