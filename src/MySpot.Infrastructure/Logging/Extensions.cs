using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using Serilog;

namespace MySpot.Infrastructure.Logging;

public static class Extensions
{
    internal static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        return services;
    }

    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, config) =>
        {
            config
                .WriteTo
                .Console()
                .WriteTo
                .File("logs/logs.txt")
                .WriteTo
                .Seq("http://localhost:5341");
        });

        return builder;
    }
}