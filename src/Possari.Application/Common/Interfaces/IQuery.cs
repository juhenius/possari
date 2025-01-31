using MediatR;
using Possari.Domain.Primitives;

namespace Possari.Application.Common.Interfaces;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
