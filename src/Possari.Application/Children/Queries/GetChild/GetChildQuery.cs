using Possari.Domain.Children;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Queries.GetChild;

public record GetChildQuery(Guid Id) : IQuery<Child>;
