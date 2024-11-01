using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public class ParkingSpotAlreadyReservedException(ParkingSpotName name, Date date): MySpotException($"Parking spot: {name} already reserved at: {date:d}.")
{
    
}