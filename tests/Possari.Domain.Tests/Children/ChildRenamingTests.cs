using Possari.Domain.Children;

namespace Possari.Domain.Tests.Children;

public class ChildRenamingTests
{
  private static readonly string validName = "name";

  [Fact]
  public void Rename_WithValidName_ChangesName()
  {
    var child = Child.Create("initial name").Value;
    var expectedName = "expected name";

    var result = child.Rename(expectedName);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, child.Name);
  }

  [Fact]
  public void Rename_WithEmptyName_Fails()
  {
    var child = Child.Create("initial name").Value;

    var result = child.Rename("");

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public void Rename_WithChangedName_RaisesChildRenamedDomainEvent()
  {
    var child = Child.Create("initial name").Value;
    child.ClearDomainEvents();

    child.Rename("new name");

    Assert.Single(child.DomainEvents);
    Assert.IsType<ChildRenamedDomainEvent>(child.DomainEvents.First());
    Assert.Equal(child.Id, ((ChildRenamedDomainEvent)child.DomainEvents.First()).ChildId);
  }

  [Fact]
  public void Rename_WithUnchangedName_DoesNotRaiseDomainEvents()
  {
    var child = Child.Create(validName).Value;
    child.ClearDomainEvents();

    child.Rename(validName);

    Assert.Empty(child.DomainEvents);
  }
}
