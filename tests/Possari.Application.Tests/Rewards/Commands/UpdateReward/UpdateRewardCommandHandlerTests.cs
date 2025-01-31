using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Rewards.Commands.UpdateReward;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;

namespace Possari.Application.Tests.Rewards.Commands.UpdateReward;

public class UpdateRewardCommandHandlerTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenCost = 1;
  private readonly IRewardRepository mockRewardRepository = Substitute.For<IRewardRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenRewardDoesNotExist_ReturnsNotFoundError()
  {
    var rewardId = Guid.NewGuid();
    var command = new UpdateRewardCommand(rewardId, validName, validTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(rewardId)
      .Returns((Reward?)null);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NotFound(rewardId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenRewardExists_UpdatesReward()
  {
    var expectedName = "new name";
    var expectedTokenCost = 5;
    var reward = TestRewardFactory.CreateReward("name", 1);
    var command = new UpdateRewardCommand(reward.Id, expectedName, expectedTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, result.Value.Name);
    Assert.Equal(expectedTokenCost, result.Value.TokenCost);

    await mockRewardRepository
      .Received(1)
      .UpdateRewardAsync(reward);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenNameIsInvalid_ReturnsError()
  {
    var invalidName = "";
    var reward = TestRewardFactory.CreateReward(validName, validTokenCost);
    var command = new UpdateRewardCommand(reward.Id, invalidName, validTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenTokenCostIsInvalid_ReturnsError()
  {
    var invalidTokenCost = -1;
    var reward = TestRewardFactory.CreateReward(validName, validTokenCost);
    var command = new UpdateRewardCommand(reward.Id, validName, invalidTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(RewardErrors.TokenCostTooLow.Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_DoesNotSaveChanges()
  {
    var invalidName = "";
    var reward = TestRewardFactory.CreateReward(validName, validTokenCost);
    var command = new UpdateRewardCommand(reward.Id, invalidName, validTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);

    await mockRewardRepository
      .DidNotReceive()
      .UpdateRewardAsync(Arg.Any<Reward>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenRewardRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var reward = TestRewardFactory.CreateReward();
    var command = new UpdateRewardCommand(reward.Id, validName, validTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .GetByIdAsync(reward.Id)
      .Returns(reward);

    mockRewardRepository
      .UpdateRewardAsync(reward)
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
    var command = new UpdateRewardCommand(reward.Id, validName, validTokenCost);
    var handler = new UpdateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

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
