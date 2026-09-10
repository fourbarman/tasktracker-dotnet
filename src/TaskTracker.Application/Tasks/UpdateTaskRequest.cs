namespace TaskTracker.Application.Tasks;

public record UpdateTaskRequest(
    string Title,
    string? Description
    );