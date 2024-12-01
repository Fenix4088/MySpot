using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.Policies;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.DomainServices;

internal sealed class ParkingReservationService(IEnumerable<IReservationPolicy> policies, IClock clock): IParkingReservationService
{

    private readonly IEnumerable<IReservationPolicy> _policies = policies;

    private readonly IClock _clock = clock;
    //Use Strategy pattern here
    //https://refactoring.guru/design-patterns/strategy/csharp/example
    public void ReserveSpotForVehicle(IEnumerable<WeeklyParkingSpot> allParkingSpots, JobTitle jobTitle, WeeklyParkingSpot parkingSpotToReserve,
        Reservation reservation)
    {
        var parkingSpotId = parkingSpotToReserve.Id;
        var policy = policies.SingleOrDefault(x => x.CanBeApplied(jobTitle));

        if (policy is null)
        {
            throw new NoReservationPolicyFoundException(jobTitle);
        }

        if (!policy.CanReserve(allParkingSpots, reservation.EmployeeName))
        {
            throw new CanNotReserveParkingSpotException(parkingSpotId);
        }
        
        parkingSpotToReserve.AddReservation(reservation, new Date(_clock.Current()));
    }
}