using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Commands.DeleteChild;

public record DeleteChildCommand(Guid Id) : ICommand;
