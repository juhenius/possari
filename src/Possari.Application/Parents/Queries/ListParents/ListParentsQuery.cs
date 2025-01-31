using Possari.Application.Common.Interfaces;
using Possari.Domain.Parents;

namespace Possari.Application.Parents.Queries.ListParents;

public record ListParentsQuery() : IQuery<List<Parent>>;
