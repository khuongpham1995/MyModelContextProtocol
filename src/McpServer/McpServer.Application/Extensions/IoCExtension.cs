using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using McpServer.Application.Behaviors;

namespace McpServer.Application.Extensions;

public static class IoCExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // Register all FluentValidation validators from this assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Register the MediatR pipeline behavior for validation
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
