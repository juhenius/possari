using Microsoft.AspNetCore.Http;
using Possari.Domain.Primitives;

namespace Possari.Presentation.Common;

public static class ErrorExtensions
{
  public static IResult ToProblemDetails(this Error error)
  {
    return Results.Problem(
      statusCode: error.GetStatusCode(),
      title: error.GetProblemTitle(),
      type: error.GetProblemType(),
      extensions: new Dictionary<string, object?> {
        { "errors", new [] { error } }
      }
    );
  }

  private static int GetStatusCode(this Error error)
  {
    return error.Type switch
    {
      ErrorType.Validation => StatusCodes.Status400BadRequest,
      ErrorType.NotFound => StatusCodes.Status404NotFound,
      ErrorType.Conflict => StatusCodes.Status409Conflict,
      _ => StatusCodes.Status500InternalServerError,
    };
  }

  private static string GetProblemTitle(this Error error)
  {
    return error.Type switch
    {
      ErrorType.Validation => "Bad Request",
      ErrorType.NotFound => "Not Found",
      ErrorType.Conflict => "Conflict",
      _ => "Server Failure",
    };
  }

  private static string GetProblemType(this Error error)
  {
    return error.Type switch
    {
      ErrorType.Validation => "https://tools.ietf.org/html/rfc9110#section-15.5.1",
      ErrorType.NotFound => "https://tools.ietf.org/html/rfc9110#section-15.5.5",
      ErrorType.Conflict => "https://tools.ietf.org/html/rfc9110#section-15.5.10",
      _ => "https://tools.ietf.org/html/rfc9110#section-15.6.1",
    };
  }
}
