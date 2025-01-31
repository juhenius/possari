using Possari.Application.Common.Interfaces;
using Possari.Domain.Rewards;

namespace Possari.Application.Rewards.Queries.ListRewards;

public record ListRewardsQuery() : IQuery<List<Reward>>;
