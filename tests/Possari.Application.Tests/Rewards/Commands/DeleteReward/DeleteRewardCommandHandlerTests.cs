using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Rewards.Commands.DeleteReward;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;

namespace Possari.Application.Tests.Rewards.Commands.DeleteReward;

public class DeleteRewardCommandHandlerTests
{
  private readonly IRewardRepository mockRewardRepository = Substitute.For<IRewardRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenRewardDoesNotExist_ReturnsNotFoundError()
  {
    var rewardId = Guid.NewGuid();
    var command = new DeleteRewardCommand(rewardId);
    var handler = new DeleteRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(rewardId)
      .Returns((Reward?)null);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NotFound(rewardId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenRewardExists_DeletesReward()
  {
    var reward = TestRewardFactory.CreateReward();
    var command = new DeleteRewardCommand(reward.Id);
    var handler = new DeleteRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsSuccess);

    await mockRewardRepository
      .Received(1)
      .RemoveRewardAsync(reward);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenRewardRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var reward = TestRewardFactory.CreateReward();
    var command = new DeleteRewardCommand(reward.Id);
    var handler = new DeleteRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    mockRewardRepository
      .RemoveRewardAsync(reward)
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, CancellationToken.None);
    });

    Assert.Equal(expectedError, exception.Message);
  }

  [Fact]
  public async Task Handle_WhenSavingChangesThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var reward = TestRewardFactory.CreateReward();
    var command = new DeleteRewardCommand(reward.Id);
    var handler = new DeleteRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    mockUnitOfWork
      .CommitChangesAsync()
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, CancellationToken.None);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
