using System.Reflection;
using Evently.Common.Application.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Application;

public static class ApplicationConfiguration
{
    public static void AddApplication(
        this IServiceCollection services,
        Assembly[] moduleAssemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(moduleAssemblies);

            config.AddOpenBehavior(typeof(RequestLoggingPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(moduleAssemblies, includeInternalTypes: true);
    }
}
