using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Parents.Queries.GetParent;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;

namespace Possari.Application.Tests.Parents.Queries.GetParent;

public class GetParentQueryHandlerTests
{
  private readonly IParentRepository mockParentRepository = Substitute.For<IParentRepository>();

  [Fact]
  public async Task Handle_WhenParentDoesNotExist_ReturnsNotFoundError()
  {
    var parentId = Guid.NewGuid();
    var command = new GetParentQuery(parentId);
    var handler = new GetParentQueryHandler(mockParentRepository);

    mockParentRepository
      .GetByIdAsync(parentId)
      .Returns((Parent?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ParentErrors.NotFound(parentId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenParentExists_ReturnsParent()
  {
    var parent = TestParentFactory.CreateParent();
    var command = new GetParentQuery(parent.Id);
    var handler = new GetParentQueryHandler(mockParentRepository);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .Returns(parent);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Equal(parent.Id, result.Value.Id);

    await mockParentRepository
      .Received(1)
      .GetByIdAsync(parent.Id);
  }

  [Fact]
  public async Task Handle_WhenParentRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var parent = TestParentFactory.CreateParent();
    var command = new GetParentQuery(parent.Id);
    var handler = new GetParentQueryHandler(mockParentRepository);

    mockParentRepository
      .GetByIdAsync(parent.Id)
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
