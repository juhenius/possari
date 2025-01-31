using Possari.Domain.Parents;

namespace Possari.Domain.Tests.Parents;

public class ParentDeletionTests
{
  private static readonly string validName = "name";

  [Fact]
  public void Delete_RaisesParentDeletedDomainEvent()
  {
    var parent = Parent.Create(validName).Value;
    parent.ClearDomainEvents();

    parent.Delete();

    Assert.Single(parent.DomainEvents);
    Assert.IsType<ParentDeletedDomainEvent>(parent.DomainEvents.First());
    Assert.Equal(parent.Id, ((ParentDeletedDomainEvent)parent.DomainEvents.First()).ParentId);
  }
}
