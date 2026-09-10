namespace TaskTracker.Application.Tasks;

//Paged task request
public record GetTasksRequest(
    int Page = 1,
    int PageSize = 20,
    bool? IsCompleted = null
);