using Possari.Domain.Children;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Children.Commands.RedeemReward;

public record RedeemRewardCommand(Guid ChildId, Guid RewardId) : ICommand<Child>;
