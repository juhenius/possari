using MediatR;
using Possari.Domain.Primitives;

namespace Possari.Application.Common.Interfaces;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
  where TCommand : ICommand;

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>>
  where TCommand : ICommand<TResult>;
