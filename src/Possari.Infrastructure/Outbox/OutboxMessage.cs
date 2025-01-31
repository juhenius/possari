namespace Possari.Infrastructure.Outbox;

public sealed class OutboxMessage
{
  public Guid Id { get; set; }
  public required string Type { get; set; } = string.Empty;
  public required string Content { get; set; } = string.Empty;
  public DateTime OccurredOnUtc { get; set; }
  public DateTime? ProcessedOnUtc { get; set; }
  public string? Error { get; set; }
}
