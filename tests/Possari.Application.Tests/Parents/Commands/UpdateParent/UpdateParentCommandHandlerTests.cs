using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Parents.Commands.UpdateParent;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;

namespace Possari.Application.Tests.Parents.Commands.UpdateParent;

public class UpdateParentCommandHandlerTests
{
  private static readonly string validName = "name";
  private readonly IParentRepository mockParentRepository = Substitute.For<IParentRepository>();
  private readonly IUnitOfWork mockUnitOfWork = Substitute.For<IUnitOfWork>();

  [Fact]
  public async Task Handle_WhenParentDoesNotExist_ReturnsNotFoundError()
  {
    var parentId = Guid.NewGuid();
    var command = new UpdateParentCommand(parentId, validName);
    var handler = new UpdateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parentId)
      .Returns((Parent?)null);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(ParentErrors.NotFound(parentId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenParentExists_UpdatesParent()
  {
    var expectedName = "new name";
    var parent = TestParentFactory.CreateParent("name");
    var command = new UpdateParentCommand(parent.Id, expectedName);
    var handler = new UpdateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsSuccess);
    Assert.Equal(expectedName, result.Value.Name);

    await mockParentRepository
      .Received(1)
      .UpdateParentAsync(parent);

    await mockUnitOfWork
      .Received(1)
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenNameIsInvalid_ReturnsError()
  {
    var invalidName = "";
    var parent = TestParentFactory.CreateParent(validName);
    var command = new UpdateParentCommand(parent.Id, invalidName);
    var handler = new UpdateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);
    Assert.Equal(ParentErrors.NameNullOrEmpty.Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenCommandIsInvalid_DoesNotSaveChanges()
  {
    var invalidName = "";
    var parent = TestParentFactory.CreateParent(validName);
    var command = new UpdateParentCommand(parent.Id, invalidName);
    var handler = new UpdateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result.IsFailure);

    await mockParentRepository
      .DidNotReceive()
      .UpdateParentAsync(Arg.Any<Parent>());

    await mockUnitOfWork
      .DidNotReceive()
      .CommitChangesAsync();
  }

  [Fact]
  public async Task Handle_WhenParentRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var parent = TestParentFactory.CreateParent();
    var command = new UpdateParentCommand(parent.Id, validName);
    var handler = new UpdateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    mockParentRepository
      .UpdateParentAsync(parent)
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
    var parent = TestParentFactory.CreateParent();
    var command = new UpdateParentCommand(parent.Id, validName);
    var handler = new UpdateParentCommandHandler(mockParentRepository, mockUnitOfWork);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

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
