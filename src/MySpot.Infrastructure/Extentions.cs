using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure;

public static class Extentions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("");
        services.Configure<AppOptions>(section);

        services.AddSingleton<ExceptionMiddleware>();
        
        return services
            .AddPostgres(configuration)
            .AddSingleton<IClock, Clock>();
        // .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();
    }

    public static WebApplication UseInfrastructure(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<ExceptionMiddleware>();
        webApplication.MapControllers();
        return webApplication;
    }
}