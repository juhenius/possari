using Possari.Domain.Parents;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Parents.Commands.CreateParent;

public record CreateParentCommand(string Name) : ICommand<Parent>;
