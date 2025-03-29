using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Parents.Commands.CreateParent;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;

namespace Possari.Application.Tests.Parents.Commands.CreateParent;

public class CreateParentCommandHandlerTests
{
  private static readonly string validName = "name";
  private readonly IParentRepository mockParentRepository = Substitute.For<IParentRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenCommandIsValid_CreatesAndReturnsParent()
  {
    var expectedName = validName;
    var command = new CreateParentCommand(expectedName);
    var handler = new CreateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.NotNull(result.Value);
    Assert.Equal(expectedName, result.Value.Name);

    await mockParentRepository
      .Received(1)
      .AddParentAsync(Arg.Is<Parent>(c => c.Id == result.Value.Id && c.Name == expectedName));

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_ReturnsError()
  {
    var invalidName = "";
    var command = new CreateParentCommand(invalidName);
    var handler = new CreateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_DoesNotSaveChanges()
  {
    var invalidName = "";
    var command = new CreateParentCommand(invalidName);
    var handler = new CreateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);

    await mockParentRepository
      .DidNotReceive()
      .AddParentAsync(Arg.Any<Parent>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenParentRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var command = new CreateParentCommand(validName);
    var handler = new CreateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .AddParentAsync(Arg.Any<Parent>())
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
    var command = new CreateParentCommand(validName);
    var handler = new CreateParentCommandHandler(mockParentRepository, mockUnitOfWork);

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
