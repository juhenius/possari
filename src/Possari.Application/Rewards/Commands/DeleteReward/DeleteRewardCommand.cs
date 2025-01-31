using Possari.Application.Common.Interfaces;

namespace Possari.Application.Rewards.Commands.DeleteReward;

public record DeleteRewardCommand(Guid Id) : ICommand;
