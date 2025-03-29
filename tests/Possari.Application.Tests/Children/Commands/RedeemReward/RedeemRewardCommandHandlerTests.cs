using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Commands.RedeemReward;
using Possari.Application.Common.Interfaces;
using Possari.Application.Tests.Rewards;
using Possari.Domain.Children;
using Possari.Domain.Rewards;

namespace Possari.Application.Tests.Children.Commands.RedeemReward;

public class RedeemRewardCommandHandlerTests
{
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();
  private readonly IRewardRepository mockRewardRepository = Substitute.For<IRewardRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenChildDoesNotExist_ReturnsNotFoundError()
  {
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns((Child?)null);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NotFound(child.Id).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenRewardDoesNotExist_ReturnsNotFoundError()
  {
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns((Reward?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NotFound(reward.Id).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenChildAndRewardExists_RedeemsReward()
  {
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    child.AwardTokens(reward.TokenCost);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);
    Assert.Contains(child.PendingRewards, r => r.RewardName == reward.Name);

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
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.InsufficientTokenBalance.Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenTokenAmountIsInvalid_DoesNotSaveChanges()
  {
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

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
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    child.AwardTokens(reward.TokenCost);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

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
    var child = TestChildFactory.CreateChild("test child");
    var reward = TestRewardFactory.CreateReward("test reward");
    var command = new RedeemRewardCommand(child.Id, reward.Id);
    var handler = new RedeemRewardCommandHandler(mockChildRepository, mockRewardRepository, mockUnitOfWork);

    child.AwardTokens(reward.TokenCost);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

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
