using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure.DAL;

// with help of DatabaseInitializer and IHostedService we are able to run code in a background, when app just started or stoped
internal sealed class DatabaseInitializer(IServiceProvider serviceProvider): IHostedService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
            dbContext.Database.Migrate();

            var weeklyParkingSpots = dbContext.WeeklyParkingSpots.ToList();

            if (!weeklyParkingSpots.Any())
            {
                var _clock = new Clock();
                weeklyParkingSpots = new List<WeeklyParkingSpot>()
                {
                    WeeklyParkingSpot.Create(
                        Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        new Week(_clock.Current()),
                        "P1"
                    ),
                    WeeklyParkingSpot.Create(
                        Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        new Week(_clock.Current()),
                        "P2"
                    ),
                    WeeklyParkingSpot.Create(
                        Guid.Parse("00000000-0000-0000-0000-000000000003"),
                        new Week(_clock.Current()),
                        "P3"
                    ),
                    WeeklyParkingSpot.Create(
                        Guid.Parse("00000000-0000-0000-0000-000000000004"),
                        new Week(_clock.Current()),
                        "P4"
                    ),
                    WeeklyParkingSpot.Create(
                        Guid.Parse("00000000-0000-0000-0000-000000000005"),
                        new Week(_clock.Current()),
                        "P5"
                    )
                };
        
                dbContext.WeeklyParkingSpots.AddRange(weeklyParkingSpots);
                dbContext.SaveChanges();
            }
        }

        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}