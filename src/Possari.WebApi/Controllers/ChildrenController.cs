using MediatR;
using Microsoft.AspNetCore.Mvc;
using Possari.Application.Children.Commands.AwardTokens;
using Possari.Application.Children.Commands.CreateChild;
using Possari.Application.Children.Commands.DeleteChild;
using Possari.Application.Children.Commands.MarkRewardAsReceived;
using Possari.Application.Children.Commands.RedeemReward;
using Possari.Application.Children.Commands.UpdateChild;
using Possari.Application.Children.Queries.GetChild;
using Possari.Application.Children.Queries.ListChildren;
using Possari.Contracts.Children;
using Possari.Domain.Children;
using Possari.WebApi.Common;

namespace Possari.WebApi.Controllers;

[Route("children")]
public class ChildrenController(ISender mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  public async Task<IActionResult> CreateChild(
    CreateChildRequest request)
  {
    var command = new CreateChildCommand(request.Name);

    var createChildResult = await _mediator.Send(command);

    return createChildResult.Match(
      child => CreatedAtAction(
        nameof(GetChild),
        new { ChildId = child.Id },
        ToChildResponse(child)),
      (_) => Problem());
  }

  [HttpPatch("{childId:guid}")]
  public async Task<IActionResult> UpdateChild(
    Guid childId,
    UpdateChildRequest request)
  {
    var command = new UpdateChildCommand(childId, request.Name);

    var updateChildResult = await _mediator.Send(command);

    return updateChildResult.Match(
      child => CreatedAtAction(
        nameof(GetChild),
        new { ChildId = child.Id },
        ToChildResponse(child)),
      (_) => Problem());
  }

  [HttpDelete("{childId:guid}")]
  public async Task<IActionResult> DeleteChild(Guid childId)
  {
    var command = new DeleteChildCommand(childId);

    var deleteChildResult = await _mediator.Send(command);

    return deleteChildResult.Match(
      () => NoContent(),
      (_) => Problem());
  }

  [HttpGet]
  public async Task<IActionResult> ListChildren()
  {
    var command = new ListChildrenQuery();

    var listChildrenResult = await _mediator.Send(command);

    return listChildrenResult.Match(
      children => Ok(children.ConvertAll(ToChildResponse)),
      (_) => Problem());
  }

  [HttpGet("{childId:guid}")]
  public async Task<IActionResult> GetChild(Guid childId)
  {
    var command = new GetChildQuery(childId);

    var getChildResult = await _mediator.Send(command);

    return getChildResult.Match(
      child => Ok(ToChildResponse(child)),
      (_) => Problem());
  }

  [HttpPost("{childId:guid}/tokens/award")]
  public async Task<IActionResult> AwardTokens(
    Guid childId,
    AwardTokensRequest request)
  {
    var command = new AwardTokensCommand(childId, request.TokenAmount);

    var awardTokensResult = await _mediator.Send(command);

    return awardTokensResult.Match(
      child => Ok(),
      (_) => Problem());
  }

  [HttpPost("{childId:guid}/rewards/redeem")]
  public async Task<IActionResult> RedeemReward(
    Guid childId,
    RedeemRewardRequest request)
  {
    var command = new RedeemRewardCommand(childId, request.RewardId);

    var redeemRewardResult = await _mediator.Send(command);

    return redeemRewardResult.Match(
      child => Ok(),
      (_) => Problem());
  }

  [HttpPatch("{childId:guid}/pending-rewards/{pendingRewardId:guid}/receive")]
  public async Task<IActionResult> RedeemReward(
    Guid childId,
    Guid pendingRewardId)
  {
    var command = new MarkRewardAsReceivedCommand(childId, pendingRewardId);

    var redeemRewardResult = await _mediator.Send(command);

    return redeemRewardResult.Match(
      child => Ok(),
      (_) => Problem());
  }

  private static ChildResponse ToChildResponse(Child child)
  {
    return new ChildResponse(
      child.Id,
      child.Name,
      child.TokenBalance,
      [.. child.PendingRewards.Select(p => new PendingRewardResponse(
        p.Id,
        p.RewardName
      ))]
      );
  }
}
