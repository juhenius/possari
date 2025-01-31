using MediatR;
using Microsoft.AspNetCore.Mvc;
using Possari.Application.Rewards.Commands.CreateReward;
using Possari.Application.Rewards.Commands.DeleteReward;
using Possari.Application.Rewards.Commands.UpdateReward;
using Possari.Application.Rewards.Queries.GetReward;
using Possari.Application.Rewards.Queries.ListRewards;
using Possari.Contracts.Rewards;
using Possari.WebApi.Common;

namespace Possari.WebApi.Controllers;

[Route("rewards")]
public class RewardsController(ISender mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  public async Task<IActionResult> CreateReward(
    CreateRewardRequest request)
  {
    var command = new CreateRewardCommand(request.Name, request.TokenCost);

    var createRewardResult = await _mediator.Send(command);

    return createRewardResult.Match(
      reward => CreatedAtAction(
        nameof(GetReward),
        new { RewardId = reward.Id },
        new RewardResponse(reward.Id, reward.Name, reward.TokenCost)),
      (_) => Problem());
  }

  [HttpPatch("{rewardId:guid}")]
  public async Task<IActionResult> UpdateReward(
      Guid rewardId,
      UpdateRewardRequest request)
  {
    var command = new UpdateRewardCommand(rewardId, request.Name, request.TokenCost);

    var updateRewardResult = await _mediator.Send(command);

    return updateRewardResult.Match(
      reward => CreatedAtAction(
        nameof(GetReward),
        new { RewardId = reward.Id },
        new RewardResponse(reward.Id, reward.Name, reward.TokenCost)),
      (_) => Problem());
  }

  [HttpDelete("{rewardId:guid}")]
  public async Task<IActionResult> DeleteReward(Guid rewardId)
  {
    var command = new DeleteRewardCommand(rewardId);

    var deleteRewardResult = await _mediator.Send(command);

    return deleteRewardResult.Match(
      () => NoContent(),
      (_) => Problem());
  }

  [HttpGet]
  public async Task<IActionResult> ListRewards()
  {
    var command = new ListRewardsQuery();

    var listRewardsResult = await _mediator.Send(command);

    return listRewardsResult.Match(
      rewards => Ok(rewards.ConvertAll(reward => new RewardResponse(reward.Id, reward.Name, reward.TokenCost))),
      (_) => Problem());
  }

  [HttpGet("{rewardId:guid}")]
  public async Task<IActionResult> GetReward(Guid rewardId)
  {
    var command = new GetRewardQuery(rewardId);

    var getRewardResult = await _mediator.Send(command);

    return getRewardResult.Match(
      reward => Ok(new RewardResponse(reward.Id, reward.Name, reward.TokenCost)),
      (_) => Problem());
  }
}
