using Possari.Domain.Children;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Commands.UpdateChild;

public record UpdateChildCommand(Guid Id, string Name) : ICommand<Child>;
