using Microsoft.EntityFrameworkCore;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Handlers;

public sealed class GetWeeklyParkingSpotsQueryHandler(MySpotDbContext mySpotDbContext): IQueryHandler<GetWeeklyParkingSpotsQuery, IEnumerable<WeeklyParkingSpotDto>>
{
    private readonly MySpotDbContext _mySpotDbContext = mySpotDbContext;
    public async Task<IEnumerable<WeeklyParkingSpotDto>> HandleAsync(GetWeeklyParkingSpotsQuery query)
    {
        var week = query.Date.HasValue ? new Week(query.Date.Value) : null;
        var weeklyParkingSpots = await _mySpotDbContext.WeeklyParkingSpots
            .Where(x => week == null || x.Week == week)
            .Include(x => x.Reservations)
            .AsNoTracking()
            .ToListAsync();

        return weeklyParkingSpots.Select(x => x.AsDto());
    }
}