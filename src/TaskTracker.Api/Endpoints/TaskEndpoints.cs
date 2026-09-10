using TaskTracker.Application.Tasks;

namespace TaskTracker.Api.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapPost("/tasks", async (
                CreateTaskRequest request,
                TaskService service,
                CancellationToken cancellationToken) =>
            {
                var task = await service.CreateAsync(request, cancellationToken);
                return Results.Created($"/tasks/{task.Id}", task);
            })
            .Produces<TaskResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);


        app.MapGet("/tasks", async (
                int? page,
                int? pageSize,
                bool? isCompleted,
                TaskService taskService,
                CancellationToken cancellationToken) =>
            {
                var request = new GetTasksRequest(
                    page ?? 1,
                    pageSize ?? 20,
                    isCompleted);

                var tasks = await taskService.GetAllPagedAsync(request, cancellationToken);

                return Results.Ok(tasks);
            })
            .Produces<PagedResponse<TaskResponse>>(StatusCodes.Status200OK);


        app.MapGet("/tasks/{id:guid}", async (
                Guid id,
                TaskService taskService,
                CancellationToken cancellationToken
            ) =>
            {
                var task = await taskService.GetByIdAsync(id, cancellationToken);

                if (task is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(task);
            })
            .Produces<TaskResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);


        app.MapPut("/tasks/{id:guid}/complete", async (
                Guid id,
                TaskService taskService,
                CancellationToken cancellationToken) =>
            {
                var completed = await taskService.CompleteAsync(id, cancellationToken);

                if (!completed)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);


        app.MapPut("/tasks/{id:guid}", async (
                Guid id,
                UpdateTaskRequest request,
                TaskService taskService,
                CancellationToken cancellationToken) =>
            {
                var task = await taskService.UpdateAsync(id, request, cancellationToken);

                if (task is null) return Results.NotFound();

                return Results.Ok(task);
            })
            .Produces<TaskResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);


        app.MapDelete("/tasks/{id:guid}", async (
                Guid id,
                TaskService taskService,
                CancellationToken cancellationToken) =>
            {
                var deleted = await taskService.DeleteAsync(id, cancellationToken);

                if (!deleted) return Results.NotFound();

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}