using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public static class RewardErrors
{
  public static Error NotFound(Guid id) => new("Rewards.NotFound", $"Reward with id = '{id}' not found");
  public static Error NameNullOrEmpty => new("Rewards.NameNullOrEmpty", "Reward name cannot be empty");
  public static Error TokenCostTooLow => new("Rewards.TokenCostTooLow", "Token cost must be greater than zero");
}
