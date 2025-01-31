using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Commands.CreateChild;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children.Commands.CreateChild;

public class CreateChildCommandHandlerTests
{
  private static readonly string validName = "name";
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenCommandIsValid_CreatesAndReturnsChild()
  {
    var expectedName = validName;
    var command = new CreateChildCommand(expectedName);
    var handler = new CreateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.NotNull(result.Value);
    Assert.Equal(expectedName, result.Value.Name);

    await mockChildRepository
      .Received(1)
      .AddChildAsync(Arg.Is<Child>(c => c.Id == result.Value.Id && c.Name == expectedName));

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_ReturnsError()
  {
    var invalidName = "";
    var command = new CreateChildCommand(invalidName);
    var handler = new CreateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_DoesNotSaveChanges()
  {
    var invalidName = "";
    var command = new CreateChildCommand(invalidName);
    var handler = new CreateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);

    await mockChildRepository
      .DidNotReceive()
      .AddChildAsync(Arg.Any<Child>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenChildRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var command = new CreateChildCommand(validName);
    var handler = new CreateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .AddChildAsync(Arg.Any<Child>())
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
    var command = new CreateChildCommand(validName);
    var handler = new CreateChildCommandHandler(mockChildRepository, mockUnitOfWork);

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
