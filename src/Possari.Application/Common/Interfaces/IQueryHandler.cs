using MediatR;
using Possari.Domain.Primitives;

namespace Possari.Application.Common.Interfaces;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
  where TQuery : IQuery<TResponse>;
