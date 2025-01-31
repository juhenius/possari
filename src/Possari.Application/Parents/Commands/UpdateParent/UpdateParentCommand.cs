using Possari.Domain.Parents;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Parents.Commands.UpdateParent;

public record UpdateParentCommand(Guid Id, string Name) : ICommand<Parent>;
