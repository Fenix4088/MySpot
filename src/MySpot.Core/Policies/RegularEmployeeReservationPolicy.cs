using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Policies;

internal sealed class RegularEmployeeReservationPolicy(IClock clock): IReservationPolicy
{
    private readonly IClock _clock = clock;
    public bool CanBeApplied(JobTitle jobTitle) => jobTitle == JobTitle.Employee;

    public bool CanReserve(IEnumerable<WeeklyParkingSpot> weeklyParkingSpots, EmployeeName employeeName)
    {
        var totalEmployeeReservations = weeklyParkingSpots.SelectMany(parkingSpot => parkingSpot.Reservations)
            .Count(x => x.EmployeeName == employeeName);

        return totalEmployeeReservations < 2 && _clock.Current().Hour > 4;
    }
}