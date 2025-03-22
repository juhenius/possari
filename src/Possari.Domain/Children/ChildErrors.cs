using Possari.Domain.Primitives;

namespace Possari.Domain.Children;

public static class ChildErrors
{
  public static Error NotFound(Guid id) => Error.NotFound("Children.NotFound", $"Child with id = '{id}' not found");
  public static Error InsufficientTokenBalance => Error.Conflict("Children.InsufficientTokenBalance", "Child does not have enough tokens to redeem reward");
  public static Error InvalidAwardTokenAmount(int amount) => Error.Validation("Children.InvalidTokenAmount", $"Token amount must be positive integer, '{amount}' is invalid");
  public static Error NameNullOrEmpty => Error.Validation("Children.NameNullOrEmpty", "Child name cannot be empty");
  public static Error PendingRewardNotFound(Guid id) => Error.NotFound("Children.PendingRewardNotFound", $"Pending reward with id = '{id}' not found");
}
