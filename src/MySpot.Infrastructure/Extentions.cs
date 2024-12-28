using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Logging;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure;

public static class Extentions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("");
        services.Configure<AppOptions>(section);

        services.AddSingleton<ExceptionMiddleware>();
        services.AddSecurity();
        
        services
            .AddPostgres(configuration)
            .AddSingleton<IClock, Clock>()
            .AddCustomLogging();
        // .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();
        
        //This logic automatically scan current assembly and register all DI into DI Container
        //It works because of Scrutor nugget package added to Application assembly
        //! Scrutor installed into Application assembly
        var infrastructureAssembly = typeof(AppOptions).Assembly;
        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<ExceptionMiddleware>();
        webApplication.MapControllers();
        return webApplication;
    }
}