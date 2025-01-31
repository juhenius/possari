using Possari.Application.Common.Interfaces;

namespace Possari.Application.Parents.Commands.DeleteParent;

public record DeleteParentCommand(Guid Id) : ICommand;
