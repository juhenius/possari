using Possari.Domain.Children;

namespace Possari.Domain.Tests.Children;

public class ChildDeletionTests
{
  private static readonly string validName = "name";

  [Fact]
  public void Delete_RaisesChildDeletedDomainEvent()
  {
    var child = Child.Create(validName).Value;
    child.ClearDomainEvents();

    child.Delete();

    Assert.Single(child.DomainEvents);
    Assert.IsType<ChildDeletedDomainEvent>(child.DomainEvents.First());
    Assert.Equal(child.Id, ((ChildDeletedDomainEvent)child.DomainEvents.First()).ChildId);
  }
}
