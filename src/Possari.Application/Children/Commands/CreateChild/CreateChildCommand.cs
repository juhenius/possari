using Possari.Domain.Children;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Commands.CreateChild;

public record CreateChildCommand(string Name) : ICommand<Child>;
