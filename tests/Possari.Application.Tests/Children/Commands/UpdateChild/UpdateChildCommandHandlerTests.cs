using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Commands.UpdateChild;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children.Commands.UpdateChild;

public class UpdateChildCommandHandlerTests
{
  private static readonly string validName = "name";
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenChildDoesNotExist_ReturnsNotFoundError()
  {
    var childId = Guid.NewGuid();
    var command = new UpdateChildCommand(childId, validName);
    var handler = new UpdateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(childId)
      .Returns((Child?)null);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NotFound(childId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenChildExists_UpdatesChild()
  {
    var expectedName = "new name";
    var child = TestChildFactory.CreateChild("name");
    var command = new UpdateChildCommand(child.Id, expectedName);
    var handler = new UpdateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, result.Value.Name);

    await mockChildRepository
      .Received(1)
      .UpdateChildAsync(child);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenNameIsInvalid_ReturnsError()
  {
    var invalidName = "";
    var child = TestChildFactory.CreateChild(validName);
    var command = new UpdateChildCommand(child.Id, invalidName);
    var handler = new UpdateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_DoesNotSaveChanges()
  {
    var invalidName = "";
    var child = TestChildFactory.CreateChild(validName);
    var command = new UpdateChildCommand(child.Id, invalidName);
    var handler = new UpdateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);

    await mockChildRepository
      .DidNotReceive()
      .UpdateChildAsync(Arg.Any<Child>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenChildRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var child = TestChildFactory.CreateChild();
    var command = new UpdateChildCommand(child.Id, validName);
    var handler = new UpdateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockChildRepository
      .UpdateChildAsync(child)
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
    var child = TestChildFactory.CreateChild();
    var command = new UpdateChildCommand(child.Id, validName);
    var handler = new UpdateChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

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
