using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Queries.GetChild;
using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;

namespace Possari.Application.Tests.Children.Queries.GetChild;

public class GetChildQueryHandlerTests
{
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();

  [Fact]
  public async Task Handle_WhenChildDoesNotExist_ReturnsNotFoundError()
  {
    var childId = Guid.NewGuid();
    var command = new GetChildQuery(childId);
    var handler = new GetChildQueryHandler(mockChildRepository);

    mockChildRepository
      .GetByIdAsync(childId)
      .Returns((Child?)null);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.True(result.IsFailure);
    Assert.Equal(ChildErrors.NotFound(childId).Code, result.Error.Code);
  }

  [Fact]
  public async Task Handle_WhenChildExists_ReturnsChild()
  {
    var child = TestChildFactory.CreateChild();
    var command = new GetChildQuery(child.Id);
    var handler = new GetChildQueryHandler(mockChildRepository);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .Returns(child);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Equal(child.Id, result.Value.Id);

    await mockChildRepository
      .Received(1)
      .GetByIdAsync(child.Id);
  }

  [Fact]
  public async Task Handle_WhenChildRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var child = TestChildFactory.CreateChild();
    var command = new GetChildQuery(child.Id);
    var handler = new GetChildQueryHandler(mockChildRepository);

    mockChildRepository
      .GetByIdAsync(child.Id)
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
