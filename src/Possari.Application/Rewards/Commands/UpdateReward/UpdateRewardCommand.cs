using Possari.Domain.Rewards;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Rewards.Commands.UpdateReward;

public record UpdateRewardCommand(Guid Id, string Name, int TokenCost) : ICommand<Reward>;
