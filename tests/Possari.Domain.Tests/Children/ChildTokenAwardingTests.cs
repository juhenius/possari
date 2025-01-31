using Possari.Domain.Children;

namespace Possari.Domain.Tests.Children;

public class ChildTokenAwardingTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenAmount = 5;

  [Fact]
  public void AwardToken_WithInvalidAmount_ReturnsError()
  {
    var child = Child.Create(validName).Value;
    var invalidTokenAmount = -1;

    var result = child.AwardTokens(invalidTokenAmount);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.InvalidAwardTokenAmount(invalidTokenAmount).Code, result.Error.Code);
  }

  [Fact]
  public void AwardToken_Succeeds_IncreasesTokenBalance()
  {
    var child = Child.Create(validName).Value;
    var expectedAmount = validTokenAmount;

    var result = child.AwardTokens(expectedAmount);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedAmount, child.TokenBalance);
  }

  [Fact]
  public void AwardToken_Succeeds_RaisesTokensAwardedDomainEvent()
  {
    var child = Child.Create(validName).Value;
    child.ClearDomainEvents();

    child.AwardTokens(validTokenAmount);

    Assert.Single(child.DomainEvents);
    Assert.IsType<TokensAwardedDomainEvent>(child.DomainEvents.First());
    Assert.Equal(child.Id, ((TokensAwardedDomainEvent)child.DomainEvents.First()).ChildId);
  }
}
