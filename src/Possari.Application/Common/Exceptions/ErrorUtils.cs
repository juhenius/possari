using Possari.Domain.Primitives;

namespace Possari.Application.Common.Exceptions;

public static class ExceptionExtensions
{
  public static Result ToResult(this Exception exception)
  {
    return new Error(nameof(exception), exception.Message);
  }

  public static Result<TValue> ToResult<TValue>(this Exception exception)
  {
    return new Error(nameof(exception), exception.Message);
  }
}
