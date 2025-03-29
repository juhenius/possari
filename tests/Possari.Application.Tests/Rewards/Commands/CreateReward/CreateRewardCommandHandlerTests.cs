using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Rewards.Commands.CreateReward;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;

namespace Possari.Application.Tests.Rewards.Commands.CreateReward;

public class CreateRewardCommandHandlerTests
{
  private static readonly string validName = "name";
  private static readonly int validTokenCost = 1;
  private readonly IRewardRepository mockRewardRepository = Substitute.For<IRewardRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenCommandIsValid_CreatesAndReturnsReward()
  {
    var expectedName = validName;
    var expectedTokenCost = validTokenCost;
    var command = new CreateRewardCommand(expectedName, expectedTokenCost);
    var handler = new CreateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.NotNull(result.Value);
    Assert.Equal(expectedName, result.Value.Name);
    Assert.Equal(expectedTokenCost, result.Value.TokenCost);

    await mockRewardRepository
      .Received(1)
      .AddRewardAsync(Arg.Is<Reward>(c => c.Id == result.Value.Id && c.Name == expectedName));

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_ReturnsError()
  {
    var invalidName = "";
    var command = new CreateRewardCommand(invalidName, validTokenCost);
    var handler = new CreateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_DoesNotSaveChanges()
  {
    var invalidName = "";
    var command = new CreateRewardCommand(invalidName, validTokenCost);
    var handler = new CreateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);

    await mockRewardRepository
      .DidNotReceive()
      .AddRewardAsync(Arg.Any<Reward>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenRewardRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var command = new CreateRewardCommand(validName, validTokenCost);
    var handler = new CreateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

    mockRewardRepository
      .AddRewardAsync(Arg.Any<Reward>())
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
    var command = new CreateRewardCommand(validName, validTokenCost);
    var handler = new CreateRewardCommandHandler(mockRewardRepository, mockUnitOfWork);

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
