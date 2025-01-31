using MediatR;
using Possari.Domain.Primitives;

namespace Possari.Application.Common.Interfaces;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
