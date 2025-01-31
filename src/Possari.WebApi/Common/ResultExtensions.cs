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

  public static IActionResult Match<T>(
    this Result<T> result,
    Func<T, IActionResult> onSuccess,
    Func<Error, IActionResult> onFailure)
  {
    return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
  }
}
