using MySpot.Application.Services;
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
            new WeeklyParkingSpot(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                new Week(_clock.Current()),
                "P1"
            ),
            new WeeklyParkingSpot(
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                new Week(_clock.Current()),
                "P2"
            ),
            new WeeklyParkingSpot(
                Guid.Parse("00000000-0000-0000-0000-000000000003"),
                new Week(_clock.Current()),
                "P3"
            ),
            new WeeklyParkingSpot(
                Guid.Parse("00000000-0000-0000-0000-000000000004"),
                new Week(_clock.Current()),
                "P4"
            ),
            new WeeklyParkingSpot(
                Guid.Parse("00000000-0000-0000-0000-000000000005"),
                new Week(_clock.Current()),
                "P5"
            )
        ];
    }

    public IEnumerable<WeeklyParkingSpot> GetAll() => _weeklyParkingSpot;

    public WeeklyParkingSpot Get(ParkingSpotId id) => _weeklyParkingSpot.SingleOrDefault(x => x.Id == id);

    public void Add(WeeklyParkingSpot weeklyParkingSpot) => _weeklyParkingSpot.Add(weeklyParkingSpot);

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
    }

    public void Delete(WeeklyParkingSpot weeklyParkingSpot)
    {
        _weeklyParkingSpot.Remove(weeklyParkingSpot);
    }
}