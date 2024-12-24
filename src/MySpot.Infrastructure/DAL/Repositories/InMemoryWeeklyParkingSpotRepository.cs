using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal class InMemoryWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly IClock _clock;
    private readonly List<WeeklyParkingSpot> _weeklyParkingSpot;
    
    public InMemoryWeeklyParkingSpotRepository(IClock clock)
    {
        _clock = clock;
        _weeklyParkingSpot = [
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
        ];
    }

    public Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync() => Task.FromResult(_weeklyParkingSpot.AsEnumerable());

    public Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id) => Task.FromResult(_weeklyParkingSpot.SingleOrDefault(x => x.Id == id));

    public Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week)
    {
        return Task.FromResult(_weeklyParkingSpot.Select(x => x).Where(x => x.Week == week));
    }

    public Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpot.Add(weeklyParkingSpot);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpot.Remove(weeklyParkingSpot);
        return Task.CompletedTask;
    }
}