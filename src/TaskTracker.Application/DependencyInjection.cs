using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Application.Tasks;

namespace TaskTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TaskService>();
        
        return services;
    }
}