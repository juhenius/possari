using Possari.Domain.Parents;

namespace Possari.Domain.Tests.Parents;

public class ParentCreationTests
{
  private static readonly string validName = "name";

  [Fact]
  public void Create_WithValidValues_SetsName()
  {
    var expectedName = "expected name";
    var result = Parent.Create(expectedName);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, result.Value.Name);
  }

  [Fact]
  public void Create_WithValidValues_UsesRandomId()
  {
    var result = Parent.Create(validName);

    Assert.True(result.IsSuccess);
    Assert.NotEqual(Guid.Empty, result.Value.Id);
  }

  [Fact]
  public void Create_WithEmptyName_Fails()
  {
    var result = Parent.Create("");

    Assert.True(result.IsFailure);
    Assert.Equal(ParentErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public void Create_WithValidValues_RaisesParentCreatedDomainEvent()
  {
    var result = Parent.Create(validName);

    Assert.True(result.IsSuccess);
    Assert.Single(result.Value.DomainEvents);
    Assert.IsType<ParentCreatedDomainEvent>(result.Value.DomainEvents.First());
    Assert.Equal(result.Value.Id, ((ParentCreatedDomainEvent)result.Value.DomainEvents.First()).ParentId);
  }
}
