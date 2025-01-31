using Possari.Domain.Parents;

namespace Possari.Domain.Tests.Parents;

public class ParentRenamingTests
{
  private static readonly string validName = "name";

  [Fact]
  public void Rename_WithValidName_ChangesName()
  {
    var parent = Parent.Create("initial name").Value;
    var expectedName = "expected name";

    var result = parent.Rename(expectedName);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, parent.Name);
  }

  [Fact]
  public void Rename_WithEmptyName_Fails()
  {
    var parent = Parent.Create("initial name").Value;

    var result = parent.Rename("");

    Assert.True(result.IsFailure);
    Assert.Equal(ParentErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public void Rename_WithChangedName_RaisesParentRenamedDomainEvent()
  {
    var parent = Parent.Create("initial name").Value;
    parent.ClearDomainEvents();

    parent.Rename("new name");

    Assert.Single(parent.DomainEvents);
    Assert.IsType<ParentRenamedDomainEvent>(parent.DomainEvents.First());
    Assert.Equal(parent.Id, ((ParentRenamedDomainEvent)parent.DomainEvents.First()).ParentId);
  }

  [Fact]
  public void Rename_WithUnchangedName_DoesNotRaiseDomainEvents()
  {
    var parent = Parent.Create(validName).Value;
    parent.ClearDomainEvents();

    parent.Rename(validName);

    Assert.Empty(parent.DomainEvents);
  }
}
