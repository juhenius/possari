using Possari.Domain.Parents;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Parents.Queries.GetParent;

public record GetParentQuery(Guid Id) : IQuery<Parent>;
