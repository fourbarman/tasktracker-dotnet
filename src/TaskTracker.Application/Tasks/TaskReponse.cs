namespace TaskTracker.Application.Tasks;

public record TaskResponse(
    Guid Id,
    string Title,
    string? Description,
    bool IsCompleted,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt
    );