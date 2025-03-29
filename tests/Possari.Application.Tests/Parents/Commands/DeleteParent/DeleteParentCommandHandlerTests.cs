using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Parents.Commands.DeleteParent;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;

namespace Possari.Application.Tests.Parents.Commands.DeleteParent;

public class DeleteParentCommandHandlerTests
{
  private readonly IParentRepository mockParentRepository = Substitute.For<IParentRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenParentDoesNotExist_ReturnsNotFoundError()
  {
    var parentId = Guid.NewGuid();
    var command = new DeleteParentCommand(parentId);
    var handler = new DeleteParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parentId)
      .Returns((Parent?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ParentErrors.NotFound(parentId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenParentExists_DeletesParent()
  {
    var parent = TestParentFactory.CreateParent();
    var command = new DeleteParentCommand(parent.Id);
    var handler = new DeleteParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsSuccess);

    await mockParentRepository
      .Received(1)
      .RemoveParentAsync(parent);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync(Arg.Any<CancellationToken>());
  }

  [Fact]
  public async Task Handle_WhenParentRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var parent = TestParentFactory.CreateParent();
    var command = new DeleteParentCommand(parent.Id);
    var handler = new DeleteParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    mockParentRepository
      .RemoveParentAsync(parent)
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
    var parent = TestParentFactory.CreateParent();
    var command = new DeleteParentCommand(parent.Id);
    var handler = new DeleteParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

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
