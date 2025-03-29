using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Commands.MarkRewardAsReceived;
using Possari.Application.Common.Interfaces;
using Possari.Application.Tests.Rewards;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children.Commands.MarkRewardAsReceived;

public class MarkRewardAsReceivedCommandHandlerTests
{
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenChildDoesNotExist_ReturnsNotFoundError()
  {
    var childId = Guid.NewGuid();
    var pendingRewardId = Guid.NewGuid();
    var command = new MarkRewardAsReceivedCommand(childId, pendingRewardId);
    var handler = new MarkRewardAsReceivedCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(childId)
      .Returns((Child?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NotFound(childId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_Succeeds_SavesChanges()
  {
    var (child, pendingRewardId) = TestChildFactory.CreateChildWithPendingReward();
    var command = new MarkRewardAsReceivedCommand(child.Id, pendingRewardId);
    var handler = new MarkRewardAsReceivedCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);
    Assert.Empty(result.Value.PendingRewards);

    await mockChildRepository
      .Received(1)
      .UpdateChildAsync(child);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_MarkingFails_ReturnsError()
  {
    var child = TestChildFactory.CreateChild();
    var pendingRewardId = Guid.NewGuid();
    var command = new MarkRewardAsReceivedCommand(child.Id, pendingRewardId);
    var handler = new MarkRewardAsReceivedCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.PendingRewardNotFound(pendingRewardId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_MarkingFails_DoesNotSaveChanges()
  {
    var child = TestChildFactory.CreateChild();
    var pendingRewardId = Guid.NewGuid();
    var command = new MarkRewardAsReceivedCommand(child.Id, pendingRewardId);
    var handler = new MarkRewardAsReceivedCommandHandler(mockChildRepository, mockUnitOfWork);

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
    var (child, pendingRewardId) = TestChildFactory.CreateChildWithPendingReward();
    var command = new MarkRewardAsReceivedCommand(child.Id, pendingRewardId);
    var handler = new MarkRewardAsReceivedCommandHandler(mockChildRepository, mockUnitOfWork);

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
    var (child, pendingRewardId) = TestChildFactory.CreateChildWithPendingReward();
    var command = new MarkRewardAsReceivedCommand(child.Id, pendingRewardId);
    var handler = new MarkRewardAsReceivedCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockUnitOfWork
      .CommitChangesAsync(Arg.Any<CancellationToken>())
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
