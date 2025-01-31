using Possari.Domain.Primitives;
using Possari.Domain.Rewards;

namespace Possari.Domain.Children;

public sealed class Child : AggregateRoot
{
  public static Result<Child> Create(string name)
  {
    var nameError = ValidateName(name);
    if (nameError != Error.None)
    {
      return nameError;
    }

    var result = new Child(name);

    result.RaiseDomainEvent(new ChildCreatedDomainEvent(result.Id));

    return result;
  }

  private readonly List<PendingReward> _pendingRewards = [];

  public string Name { get; private set; } = null!;
  public int TokenBalance { get; private set; } = 0;
  public IReadOnlyCollection<PendingReward> PendingRewards => _pendingRewards.AsReadOnly();


  private Child(string name, Guid? id = null) : base(id)
  {
    Name = name;
  }

  private Child() { }

  public Result AwardTokens(int tokenAmount)
  {
    var error = ValidateAwardTokenAmount(tokenAmount);
    if (error != Error.None)
    {
      return error;
    }

    TokenBalance += tokenAmount;

    RaiseDomainEvent(new TokensAwardedDomainEvent(Id, tokenAmount));

    return Result.Success();
  }

  public Result RedeemReward(Reward reward)
  {
    if (TokenBalance < reward.TokenCost)
    {
      return ChildErrors.InsufficientTokenBalance;
    }

    var pendingRewardResult = PendingReward.Create(this, reward);
    if (pendingRewardResult.IsFailure)
    {
      return pendingRewardResult.Error;
    }

    TokenBalance -= reward.TokenCost;
    _pendingRewards.Add(pendingRewardResult.Value);

    RaiseDomainEvent(new RewardRedeemedDomainEvent(Id, reward.Id));

    return Result.Success();
  }

  public Result MarkRewardAsReceived(Guid pendingRewardId)
  {
    var pendingReward = _pendingRewards.FirstOrDefault(r => r.Id == pendingRewardId);
    if (pendingReward is null)
    {
      return ChildErrors.PendingRewardNotFound(pendingRewardId);
    }

    _pendingRewards.Remove(pendingReward);

    RaiseDomainEvent(new PendingRewardReceivedDomainEvent(Id, pendingReward.RewardName));

    return Result.Success();
  }

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

    RaiseDomainEvent(new ChildRenamedDomainEvent(Id, Name, previousName));

    return Result.Success();
  }

  public Result Delete()
  {
    RaiseDomainEvent(new ChildDeletedDomainEvent(Id));
    return Result.Success();
  }

  private static Error ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
    {
      return ChildErrors.NameNullOrEmpty;
    }

    return Error.None;
  }

  private static Error ValidateAwardTokenAmount(int tokenAmount)
  {
    if (tokenAmount <= 0)
    {
      return ChildErrors.InvalidAwardTokenAmount(tokenAmount);
    }

    return Error.None;
  }
}
