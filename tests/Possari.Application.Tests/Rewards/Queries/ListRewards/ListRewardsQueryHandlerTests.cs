using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Rewards.Queries.ListRewards;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Tests.Rewards.Queries.ListRewards;

public class ListRewardsQueryHandlerTests
{
  private readonly IRewardRepository mockRewardRepository = Substitute.For<IRewardRepository>();

  [Fact]
  public async Task Handle_WhenRepositoryIsEmpty_ReturnsEmptyList()
  {
    var command = new ListRewardsQuery();
    var handler = new ListRewardsQueryHandler(mockRewardRepository);

    mockRewardRepository
      .ListAsync()
      .Returns([]);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Empty(result.Value);
  }

  [Fact]
  public async Task Handle_WhenRewardsExist_ReturnsAllRewards()
  {
    var rewards = TestRewardFactory.CreateMultipleRewards(2);
    var command = new ListRewardsQuery();
    var handler = new ListRewardsQueryHandler(mockRewardRepository);

    mockRewardRepository
      .ListAsync()
      .Returns(rewards);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Equal(rewards.Count, result.Value.Count);
    Assert.Equivalent(rewards, result.Value);
  }

  [Fact]
  public async Task Handle_WhenRewardRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var command = new ListRewardsQuery();
    var handler = new ListRewardsQueryHandler(mockRewardRepository);

    mockRewardRepository
      .ListAsync()
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
