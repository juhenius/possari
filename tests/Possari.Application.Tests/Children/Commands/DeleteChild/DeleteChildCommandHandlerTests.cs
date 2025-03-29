using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Commands.DeleteChild;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children.Commands.DeleteChild;

public class DeleteChildCommandHandlerTests
{
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenChildDoesNotExist_ReturnsNotFoundError()
  {
    var childId = Guid.NewGuid();
    var command = new DeleteChildCommand(childId);
    var handler = new DeleteChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(childId)
      .Returns((Child?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NotFound(childId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenChildExists_DeletesChild()
  {
    var child = TestChildFactory.CreateChild();
    var command = new DeleteChildCommand(child.Id);
    var handler = new DeleteChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);

    await mockChildRepository
      .Received(1)
      .RemoveChildAsync(child);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenChildRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var child = TestChildFactory.CreateChild();
    var command = new DeleteChildCommand(child.Id);
    var handler = new DeleteChildCommandHandler(mockChildRepository, mockUnitOfWork);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    mockChildRepository
      .RemoveChildAsync(child)
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
    var child = TestChildFactory.CreateChild();
    var command = new DeleteChildCommand(child.Id);
    var handler = new DeleteChildCommandHandler(mockChildRepository, mockUnitOfWork);

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
