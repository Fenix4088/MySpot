using Microsoft.Extensions.DependencyInjection;

namespace MySpot.Core;

public static class Extentions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}