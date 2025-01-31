using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public static class ChildErrors
{
  public static Error NotFound(Guid id) => new("Children.NotFound", $"Child with id = '{id}' not found");
  public static Error InsufficientTokenBalance => new("Children.InsufficientTokenBalance", "Child does not have enough tokens to redeem reward");
  public static Error InvalidAwardTokenAmount(int amount) => new("Children.InvalidTokenAmount", $"Token amount must be positive integer, '{amount}' is invalid");
  public static Error NameNullOrEmpty => new("Children.NameNullOrEmpty", "Child name cannot be empty");
  public static Error PendingRewardNotFound(Guid id) => new("Children.PendingRewardNotFound", $"Pending reward with id = '{id}' not found");
}
