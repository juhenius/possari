using MediatR;
using Microsoft.AspNetCore.Mvc;
using Possari.Application.Parents.Commands.CreateParent;
using Possari.Application.Parents.Commands.DeleteParent;
using Possari.Application.Parents.Commands.UpdateParent;
using Possari.Application.Parents.Queries.GetParent;
using Possari.Application.Parents.Queries.ListParents;
using Possari.Contracts.Parents;
using Possari.Domain.Parents;
using Possari.WebApi.Common;

namespace Possari.WebApi.Controllers;

[Route("parents")]
public class ParentsController(ISender mediator) : ApiController
{
  private readonly ISender _mediator = mediator;

  [HttpPost]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ParentResponse), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<ParentResponse>> CreateParent(
    CreateParentRequest request)
  {
    var command = new CreateParentCommand(request.Name);

    var createParentResult = await _mediator.Send(command);

    return createParentResult.Match<Parent, ParentResponse>(
      parent => CreatedAtAction(
        nameof(GetParent),
        new { ParentId = parent.Id },
        ToParentResponse(parent)),
      (_) => Problem());
  }

  [HttpPatch("{parentId:guid}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ParentResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<ParentResponse>> UpdateParent(
    Guid parentId,
    UpdateParentRequest request)
  {
    var command = new UpdateParentCommand(parentId, request.Name);

    var updateParentResult = await _mediator.Send(command);

    return updateParentResult.Match<Parent, ParentResponse>(
      parent => Ok(ToParentResponse(parent)),
      (_) => Problem());
  }

  [HttpDelete("{parentId:guid}")]
  [Produces("application/json")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> DeleteParent(Guid parentId)
  {
    var command = new DeleteParentCommand(parentId);

    var deleteParentResult = await _mediator.Send(command);

    return deleteParentResult.Match(
      () => NoContent(),
      (_) => Problem());
  }

  [HttpGet]
  [Produces("application/json")]
  [ProducesResponseType(typeof(List<ParentResponse>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<List<ParentResponse>>> ListParents()
  {
    var command = new ListParentsQuery();

    var listParentsResult = await _mediator.Send(command);

    return listParentsResult.Match<List<Parent>, List<ParentResponse>>(
      parents => Ok(parents.ConvertAll(ToParentResponse)),
      (_) => Problem());
  }

  [HttpGet("{parentId:guid}")]
  [Produces("application/json")]
  [ProducesResponseType(typeof(ParentResponse), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status500InternalServerError)]
  public async Task<ActionResult<ParentResponse>> GetParent(Guid parentId)
  {
    var command = new GetParentQuery(parentId);

    var getParentResult = await _mediator.Send(command);

    return getParentResult.Match<Parent, ParentResponse>(
      parent => Ok(ToParentResponse(parent)),
      (_) => Problem());
  }

  private static ParentResponse ToParentResponse(Parent parent)
  {
    return new ParentResponse(parent.Id, parent.Name);
  }
}
