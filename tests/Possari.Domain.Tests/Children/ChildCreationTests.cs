using Possari.Domain.Children;

namespace Possari.Domain.Tests.Children;

public class ChildCreationTests
{
  private static readonly string validName = "name";

  [Fact]
  public void Create_WithValidValues_SetsName()
  {
    var expectedName = "expected name";
    var result = Child.Create(expectedName);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, result.Value.Name);
  }

  [Fact]
  public void Create_WithValidValues_UsesRandomId()
  {
    var result = Child.Create(validName);

    Assert.True(result.IsSuccess);
    Assert.NotEqual(Guid.Empty, result.Value.Id);
  }

  [Fact]
  public void Create_WithValidValues_StartsWithTokenBalanceZero()
  {
    var result = Child.Create(validName);

    Assert.True(result.IsSuccess);
    Assert.Equal(0, result.Value.TokenBalance);
  }

  [Fact]
  public void Create_WithEmptyName_Fails()
  {
    var result = Child.Create("");

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public void Create_WithValidValues_RaisesChildCreatedDomainEvent()
  {
    var result = Child.Create(validName);

    Assert.True(result.IsSuccess);
    Assert.Single(result.Value.DomainEvents);
    Assert.IsType<ChildCreatedDomainEvent>(result.Value.DomainEvents.First());
    Assert.Equal(result.Value.Id, ((ChildCreatedDomainEvent)result.Value.DomainEvents.First()).ChildId);
  }
}
