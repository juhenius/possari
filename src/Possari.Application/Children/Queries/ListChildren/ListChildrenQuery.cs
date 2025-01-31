using Possari.Application.Common.Interfaces;
using Possari.Domain.Children;

namespace Possari.Application.Children.Queries.ListChildren;

public record ListChildrenQuery() : IQuery<List<Child>>;
