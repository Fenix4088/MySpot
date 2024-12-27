using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal sealed class PostgresWeeklyParkingSpotRepository(MySpotDbContext mySpotDbContext): IWeeklyParkingSpotRepository
{
    private readonly MySpotDbContext _mySpotDbContext = mySpotDbContext;

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
    {
        var result = await _mySpotDbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .ToListAsync();

        return result.AsEnumerable();
    }

    public Task<WeeklyParkingSpot> GetAsync(ParkingSpotId id) => _mySpotDbContext.WeeklyParkingSpots
        .Include(x => x.Reservations)
        .SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week) => await mySpotDbContext
        .WeeklyParkingSpots
        .Include(x => x.Reservations)
        .Where(x => x.Week == week)
        .ToListAsync();

    public async Task AddAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        await _mySpotDbContext.AddAsync(weeklyParkingSpot);
    }

    public Task UpdateAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _mySpotDbContext.Update(weeklyParkingSpot);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(WeeklyParkingSpot weeklyParkingSpot)
    {
        _mySpotDbContext.Remove(weeklyParkingSpot);
        await _mySpotDbContext.SaveChangesAsync();
    }
}