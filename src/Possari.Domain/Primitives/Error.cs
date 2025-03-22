namespace Possari.Domain.Primitives;

public record Error
{
  public static readonly Error None = Failure(string.Empty, string.Empty);

  public static readonly Error NullValue = Failure("Error.NullValue", "The specified result value is null.");

  public static readonly Error ConditionNotMet = Failure("Error.ConditionNotMet", "The specified condition was not met.");

  private Error(string code, string message, ErrorType type)
  {
    Code = code;
    Message = message;
    Type = type;
  }

  public string Code { get; }
  public string Message { get; }
  public ErrorType Type { get; }

  public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
  public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
  public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
  public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
}
