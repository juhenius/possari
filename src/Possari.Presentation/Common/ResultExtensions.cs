using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Possari.Domain.Primitives;

namespace Possari.Presentation.Common;

public static class ResultExtensions
{
  public static IResult Match(
    this Result result,
    Func<IResult> onSuccess,
    Func<Error, IResult> onFailure)
  {
    return result.IsSuccess ? onSuccess() : onFailure(result.Error);
  }

  public static IResult Match<TResult>(
    this Result<TResult> result,
    Func<TResult, IResult> onSuccess,
    Func<Error, IResult> onFailure)
  {
    return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
  }
}
