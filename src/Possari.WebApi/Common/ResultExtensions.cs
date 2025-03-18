using Microsoft.AspNetCore.Mvc;
using Possari.Domain.Primitives;

namespace Possari.WebApi.Common;

public static class ResultExtensions
{
  public static IActionResult Match(
    this Result result,
    Func<IActionResult> onSuccess,
    Func<Error, IActionResult> onFailure)
  {
    return result.IsSuccess ? onSuccess() : onFailure(result.Error);
  }

  public static ActionResult<TActionResult> Match<TActionResult>(
    this Result result,
    Func<ActionResult<TActionResult>> onSuccess,
    Func<Error, ActionResult<TActionResult>> onFailure)
  {
    return result.IsSuccess ? onSuccess() : onFailure(result.Error);
  }

  public static IActionResult Match<TResult>(
    this Result<TResult> result,
    Func<TResult, IActionResult> onSuccess,
    Func<Error, IActionResult> onFailure)
  {
    return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
  }

  public static ActionResult<TActionResult> Match<TResult, TActionResult>(
    this Result<TResult> result,
    Func<TResult, ActionResult<TActionResult>> onSuccess,
    Func<Error, ActionResult<TActionResult>> onFailure)
  {
    return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
  }
}
