using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public static class RewardErrors
{
  public static Error NotFound(Guid id) => Error.NotFound("Rewards.NotFound", $"Reward with id = '{id}' not found");
  public static Error NameNullOrEmpty => Error.Validation("Rewards.NameNullOrEmpty", "Reward name cannot be empty");
  public static Error TokenCostTooLow => Error.Validation("Rewards.TokenCostTooLow", "Token cost must be greater than zero");
}
