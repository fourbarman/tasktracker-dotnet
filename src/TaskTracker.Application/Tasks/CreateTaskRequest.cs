namespace TaskTracker.Application.Tasks;

public record CreateTaskRequest(
    string Title,
    string? Description);