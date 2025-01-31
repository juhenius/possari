using Possari.Domain.Primitives;

namespace Possari.Domain.Rewards;

public sealed class Reward : AggregateRoot
{
  public static Result<Reward> Create(string name, int tokenCost)
  {
    var nameError = ValidateName(name);
    if (nameError != Error.None)
    {
      return nameError;
    }

    var tokenCostError = ValidateTokenCost(tokenCost);
    if (tokenCostError != Error.None)
    {
      return tokenCostError;
    }

    var result = new Reward(name, tokenCost);

    result.RaiseDomainEvent(new RewardCreatedDomainEvent(result.Id));

    return result;
  }

  public string Name { get; private set; } = null!;
  public int TokenCost { get; private set; }

  private Reward(string name, int tokenCost, Guid? id = null) : base(id)
  {
    Name = name;
    TokenCost = tokenCost;
  }

  private Reward() { }

  public Result Rename(string name)
  {
    var error = ValidateName(name);
    if (error != Error.None)
    {
      return error;
    }

    if (Name == name)
    {
      return Result.Success();
    }

    var previousName = Name;
    Name = name;

    RaiseDomainEvent(new RewardRenamedDomainEvent(Id, Name, previousName));

    return Result.Success();
  }

  public Result UpdateTokenCost(int tokenCost)
  {
    var error = ValidateTokenCost(tokenCost);
    if (error != Error.None)
    {
      return error;
    }

    if (TokenCost == tokenCost)
    {
      return Result.Success();
    }

    var previousTokenCost = TokenCost;
    TokenCost = tokenCost;
    RaiseDomainEvent(new RewardTokenCostUpdated(Id, TokenCost, previousTokenCost));

    return Result.Success();
  }

  public Result Delete()
  {
    RaiseDomainEvent(new RewardDeletedDomainEvent(Id));
    return Result.Success();
  }

  private static Error ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
    {
      return RewardErrors.NameNullOrEmpty;
    }

    return Error.None;
  }

  private static Error ValidateTokenCost(int tokenCost)
  {
    if (tokenCost <= 0)
    {
      return RewardErrors.TokenCostTooLow;
    }

    return Error.None;
  }
}
