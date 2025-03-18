using MediatR;
using Microsoft.AspNetCore.Mvc;
using Possari.Application.Rewards.Commands.CreateReward;
using Possari.Application.Rewards.Commands.DeleteReward;
using Possari.Application.Rewards.Commands.UpdateReward;
using Possari.Application.Rewards.Queries.GetReward;
using Possari.Application.Rewards.Queries.ListRewards;
using Possari.Contracts.Rewards;
using Possari.Domain.Rewards;
using Possari.WebApi.Common;

namespace Possari.WebApi.Controllers;

[Route("rewards")]
public class RewardsController(ISender mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  [Produces("application/json")]
  [ProducesResponseType(typeof(RewardResponse), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<RewardResponse>> CreateReward(
    CreateRewardRequest request)
  {
    var command = new CreateRewardCommand(request.Name, request.TokenCost);

    var createRewardResult = await _mediator.Send(command);

    return createRewardResult.Match<Reward, RewardResponse>(
      reward => CreatedAtAction(
        nameof(GetReward),
        new { RewardId = reward.Id },
        ToRewardResponse(reward)),
      (_) => Problem());
  }

  [HttpPatch("{rewardId:guid}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(RewardResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<RewardResponse>> UpdateReward(
    Guid rewardId,
    UpdateRewardRequest request)
  {
    var command = new UpdateRewardCommand(rewardId, request.Name, request.TokenCost);

    var updateRewardResult = await _mediator.Send(command);

    return updateRewardResult.Match<Reward, RewardResponse>(
      reward => Ok(ToRewardResponse(reward)),
      (_) => Problem());
  }

  [HttpDelete("{rewardId:guid}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteReward(Guid rewardId)
  {
    var command = new DeleteRewardCommand(rewardId);

    var deleteRewardResult = await _mediator.Send(command);

    return deleteRewardResult.Match(
      () => NoContent(),
      (_) => Problem());
  }

  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(List<RewardResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<List<RewardResponse>>> ListRewards()
  {
    var command = new ListRewardsQuery();

    var listRewardsResult = await _mediator.Send(command);

    return listRewardsResult.Match<List<Reward>, List<RewardResponse>>(
      rewards => Ok(rewards.ConvertAll(ToRewardResponse)),
      (_) => Problem());
  }

  [HttpGet("{rewardId:guid}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(RewardResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<RewardResponse>> GetReward(Guid rewardId)
  {
    var command = new GetRewardQuery(rewardId);

    var getRewardResult = await _mediator.Send(command);

    return getRewardResult.Match<Reward, RewardResponse>(
      reward => Ok(ToRewardResponse(reward)),
      (_) => Problem());
  }

  private static RewardResponse ToRewardResponse(Reward reward)
  {
    return new RewardResponse(reward.Id, reward.Name, reward.TokenCost);
  }
}
