using Possari.Domain.Children;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Commands.MarkRewardAsReceived;

public record MarkRewardAsReceivedCommand(Guid Id, Guid PendingRewardId) : ICommand<Child>;
