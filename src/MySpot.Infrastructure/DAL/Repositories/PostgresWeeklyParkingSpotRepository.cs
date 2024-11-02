using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal sealed class PostgresWeeklyParkingSpotRepository(MySpotDbContext mySpotDbContext): IWeeklyParkingSpotRepository
{
    private readonly MySpotDbContext _mySpotDbContext = mySpotDbContext;

    public IEnumerable<WeeklyParkingSpot> GetAll() => _mySpotDbContext.WeeklyParkingSpots
        .Include(x => x.Reservations)
        .ToList();
    
    public WeeklyParkingSpot Get(ParkingSpotId id) => _mySpotDbContext.WeeklyParkingSpots
        .Include(x => x.Reservations)
        .SingleOrDefault(x => x.Id == id);

    public void Add(WeeklyParkingSpot weeklyParkingSpot)
    {
        _mySpotDbContext.Add(weeklyParkingSpot);
        _mySpotDbContext.SaveChanges();
    }

    public void Update(WeeklyParkingSpot weeklyParkingSpot)
    {
        _mySpotDbContext.Update(weeklyParkingSpot);
        _mySpotDbContext.SaveChanges();
    }

    public void Delete(WeeklyParkingSpot weeklyParkingSpot)
    {
        _mySpotDbContext.Remove(weeklyParkingSpot);
        _mySpotDbContext.SaveChanges();
    }
}