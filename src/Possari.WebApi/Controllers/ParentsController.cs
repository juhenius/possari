using MediatR;
using Microsoft.AspNetCore.Mvc;
using Possari.Application.Parents.Commands.CreateParent;
using Possari.Application.Parents.Commands.DeleteParent;
using Possari.Application.Parents.Commands.UpdateParent;
using Possari.Application.Parents.Queries.GetParent;
using Possari.Application.Parents.Queries.ListParents;
using Possari.Contracts.Parents;
using Possari.WebApi.Common;

namespace Possari.WebApi.Controllers;

[Route("parents")]
public class ParentsController(ISender mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  public async Task<IActionResult> CreateParent(
    CreateParentRequest request)
  {
    var command = new CreateParentCommand(request.Name);

    var createParentResult = await _mediator.Send(command);

    return createParentResult.Match(
      parent => CreatedAtAction(
        nameof(GetParent),
        new { ParentId = parent.Id },
        new ParentResponse(parent.Id, parent.Name)),
      (_) => Problem());
  }

  [HttpPatch("{parentId:guid}")]
  public async Task<IActionResult> UpdateParent(
      Guid parentId,
      UpdateParentRequest request)
  {
    var command = new UpdateParentCommand(parentId, request.Name);

    var updateParentResult = await _mediator.Send(command);

    return updateParentResult.Match(
      parent => CreatedAtAction(
        nameof(GetParent),
        new { ParentId = parent.Id },
        new ParentResponse(parent.Id, parent.Name)),
      (_) => Problem());
  }

  [HttpDelete("{parentId:guid}")]
  public async Task<IActionResult> DeleteParent(Guid parentId)
  {
    var command = new DeleteParentCommand(parentId);

    var deleteParentResult = await _mediator.Send(command);

    return deleteParentResult.Match(
      () => NoContent(),
      (_) => Problem());
  }

  [HttpGet]
  public async Task<IActionResult> ListParents()
  {
    var command = new ListParentsQuery();

    var listParentsResult = await _mediator.Send(command);

    return listParentsResult.Match(
      parents => Ok(parents.ConvertAll(parent => new ParentResponse(parent.Id, parent.Name))),
      (_) => Problem());
  }

  [HttpGet("{parentId:guid}")]
  public async Task<IActionResult> GetParent(Guid parentId)
  {
    var command = new GetParentQuery(parentId);

    var getParentResult = await _mediator.Send(command);

    return getParentResult.Match(
      parent => Ok(new ParentResponse(parent.Id, parent.Name)),
      (_) => Problem());
  }
}
