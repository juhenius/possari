using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Possari.Application.Parents.Queries.ListParents;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Tests.Parents.Queries.ListParents;

public class ListParentsQueryHandlerTests
{
  private readonly IParentRepository mockParentRepository = Substitute.For<IParentRepository>();

  [Fact]
  public async Task Handle_WhenRepositoryIsEmpty_ReturnsEmptyList()
  {
    var command = new ListParentsQuery();
    var handler = new ListParentsQueryHandler(mockParentRepository);

    mockParentRepository
      .ListAsync()
      .Returns([]);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.Empty(result.Value);
  }

  [Fact]
  public async Task Handle_WhenParentsExist_ReturnsAllParents()
  {
    var parents = TestParentFactory.CreateMultipleParents(2);
    var command = new ListParentsQuery();
    var handler = new ListParentsQueryHandler(mockParentRepository);

    mockParentRepository
      .ListAsync()
      .Returns(parents);

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.Equal(parents.Count, result.Value.Count);
    Assert.Equivalent(parents, result.Value);
  }

  [Fact]
  public async Task Handle_WhenParentRepositoryThrowsException_PropagatesError()
  {
    var expectedError = "expected error";
    var command = new ListParentsQuery();
    var handler = new ListParentsQueryHandler(mockParentRepository);

    mockParentRepository
      .ListAsync()
      .ThrowsAsync(new Exception(expectedError));

    var exception = await Assert.ThrowsAsync<Exception>(() =>
    {
      return handler.Handle(command, CancellationToken.None);
    });

    Assert.Equal(expectedError, exception.Message);
  }
}
