using Possari.Domain.Rewards;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Rewards.Commands.CreateReward;

public record CreateRewardCommand(string Name, int TokenCost) : ICommand<Reward>;
