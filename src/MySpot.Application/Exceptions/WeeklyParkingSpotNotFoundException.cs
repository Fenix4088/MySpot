using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class WeeklyParkingSpotNotFoundException : MySpotException
{

    public WeeklyParkingSpotNotFoundException() : base("Weekly parking spot not found.")
    {
    }

    public WeeklyParkingSpotNotFoundException(Guid parkingSpotId) : base($"Weekly parking spot with id: {parkingSpotId} was not found.")
    {
    }
}