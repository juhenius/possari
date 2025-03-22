namespace Possari.Presentation.Endpoints;

public static class ApiEndpoints
{
  private const string ApiBase = "";

  public static class Children
  {
    private const string Base = $"{ApiBase}/children";
    public const string Create = Base;
    public const string List = Base;
    public const string Get = $"{Base}/{{childId:guid}}";
    public const string Update = $"{Base}/{{childId:guid}}";
    public const string Delete = $"{Base}/{{childId:guid}}";
    public const string AwardTokens = $"{Base}/{{childId:guid}}/award-tokens";
    public const string RedeemReward = $"{Base}/{{childId:guid}}/redeem-reward";
    public const string ReceiveReward = $"{Base}/{{childId:guid}}/receive-reward/{{rewardId:guid}}";
  }

  public static class Parents
  {
    private const string Base = $"{ApiBase}/parents";
    public const string Create = Base;
    public const string List = Base;
    public const string Get = $"{Base}/{{parentId:guid}}";
    public const string Update = $"{Base}/{{parentId:guid}}";
    public const string Delete = $"{Base}/{{parentId:guid}}";
  }

  public static class Rewards
  {
    private const string Base = $"{ApiBase}/rewards";
    public const string Create = Base;
    public const string List = Base;
    public const string Get = $"{Base}/{{rewardId:guid}}";
    public const string Update = $"{Base}/{{rewardId:guid}}";
    public const string Delete = $"{Base}/{{rewardId:guid}}";
  }
}
