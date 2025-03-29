using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Rewards.Queries.GetReward;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;

namespace Possari.Application.Tests.Rewards.Queries.GetReward;

public class GetRewardQueryHandlerTests
{
  private readonly IRewardRepository mockRewardRepository = Substitute.For<IRewardRepository>();

  [Fact]
  public async Task Handle_WhenRewardDoesNotExist_ReturnsNotFoundError()
  {
    var rewardId = Guid.NewGuid();
    var command = new GetRewardQuery(rewardId);
    var handler = new GetRewardQueryHandler(mockRewardRepository);

    mockRewardRepository
      .GetByIdAsync(rewardId)
      .Returns((Reward?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NotFound(rewardId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenRewardExists_ReturnsReward()
  {
    var reward = TestRewardFactory.CreateReward();
    var command = new GetRewardQuery(reward.Id);
    var handler = new GetRewardQueryHandler(mockRewardRepository);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Equal(reward.Id, result.Value.Id);

    await mockRewardRepository
      .Received(1)
      .GetByIdAsync(reward.Id);
  }

  [Fact]
  public async Task Handle_WhenRewardRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var reward = TestRewardFactory.CreateReward();
    var command = new GetRewardQuery(reward.Id);
    var handler = new GetRewardQueryHandler(mockRewardRepository);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
