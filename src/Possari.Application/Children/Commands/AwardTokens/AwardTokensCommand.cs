using Possari.Domain.Children;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Commands.AwardTokens;

public record AwardTokensCommand(Guid Id, int TokenAmount) : ICommand<Child>;
