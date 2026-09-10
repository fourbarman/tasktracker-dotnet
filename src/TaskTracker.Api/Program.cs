using TaskTracker.Api.Endpoints;
using TaskTracker.Api.ExceptionHandlers;
using TaskTracker.Application;
using TaskTracker.Application.Common;
using TaskTracker.Application.Tasks;
using TaskTracker.Infrastructure;
using TaskTracker.Infrastructure.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add exception handler
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<DomainValidationExceptionHandler>();
builder.Services.AddExceptionHandler<ApplicationValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

//middleware: exceptionHandlers etc...
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        service = "TaskTracker.Api",
        status = "running"
    });
});

app.MapTaskEndpoints();

app.Run();