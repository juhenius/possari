using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Children.Queries.ListChildren;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Tests.Children.Queries.ListChildren;

public class ListChildrenQueryHandlerTests
{
  private readonly IChildRepository mockChildRepository = Substitute.For<IChildRepository>();

  [Fact]
  public async Task Handle_WhenRepositoryIsEmpty_ReturnsEmptyList()
  {
    var command = new ListChildrenQuery();
    var handler = new ListChildrenQueryHandler(mockChildRepository);

    mockChildRepository
      .ListAsync()
      .Returns([]);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Empty(result.Value);
  }

  [Fact]
  public async Task Handle_WhenChildrenExist_ReturnsAllChildren()
  {
    var children = TestChildFactory.CreateMultipleChildren(2);
    var command = new ListChildrenQuery();
    var handler = new ListChildrenQueryHandler(mockChildRepository);

    mockChildRepository
      .ListAsync()
      .Returns(children);

    var result = await handler.Handle(command, TestContext.Current.CancellationToken);

    Assert.Equal(children.Count, result.Value.Count);
    Assert.Equivalent(children, result.Value);
  }

  [Fact]
  public async Task Handle_WhenChildRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var command = new ListChildrenQuery();
    var handler = new ListChildrenQueryHandler(mockChildRepository);

    mockChildRepository
      .ListAsync()
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, TestContext.Current.CancellationToken);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
