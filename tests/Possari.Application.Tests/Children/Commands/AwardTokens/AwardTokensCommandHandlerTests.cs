using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Commands.AwardTokens;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children.Commands.AwardTokens;

public class AwardTokensCommandHandlerTests
{
  private static readonly int validTokenAmount = 1;
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenChildDoesNotExist_ReturnsNotFoundError()
  {
    var childId = Guid.NewGuid();
    var command = new AwardTokensCommand(childId, validTokenAmount);
    var handler = new AwardTokensCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(childId)
      .Returns((Child?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NotFound(childId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenChildExists_AwardsTokens()
  {
    var expectedTokenBalance = 5;
    var child = TestChildFactory.CreateChild("name");
    var command = new AwardTokensCommand(child.Id, expectedTokenBalance);
    var handler = new AwardTokensCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedTokenBalance, result.Value.TokenBalance);

    await mockChildRepository
      .Received(1)
      .UpdateChildAsync(child);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenTokenAmountIsInvalid_ReturnsError()
  {
    var invalidTokenAmount = -1;
    var child = TestChildFactory.CreateChild();
    var command = new AwardTokensCommand(child.Id, invalidTokenAmount);
    var handler = new AwardTokensCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.InvalidAwardTokenAmount(invalidTokenAmount).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenTokenAmountIsInvalid_DoesNotSaveChanges()
  {
    var invalidTokenAmount = -1;
    var child = TestChildFactory.CreateChild();
    var command = new AwardTokensCommand(child.Id, invalidTokenAmount);
    var handler = new AwardTokensCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);

    await mockChildRepository
      .DidNotReceive()
      .UpdateChildAsync(Arg.Any<Child>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenChildRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var child = TestChildFactory.CreateChild();
    var command = new AwardTokensCommand(child.Id, validTokenAmount);
    var handler = new AwardTokensCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockChildRepository
      .UpdateChildAsync(child)
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }

  [Fact]
  public async Task Handle_WhenSavingChangesThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var child = TestChildFactory.CreateChild();
    var command = new AwardTokensCommand(child.Id, validTokenAmount);
    var handler = new AwardTokensCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockUnitOfWork
      .CommitChangesAsync(TestContext.Current.CancellationToken)
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
