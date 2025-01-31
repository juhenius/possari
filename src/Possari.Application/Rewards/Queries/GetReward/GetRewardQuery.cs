using Possari.Domain.Rewards;
using Possari.Application.Common.Interfaces;

namespace Possari.Application.Rewards.Queries.GetReward;

public record GetRewardQuery(Guid Id) : IQuery<Reward>;
