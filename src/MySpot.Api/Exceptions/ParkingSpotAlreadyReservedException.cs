using MySpot.Api.ValueObjects;

namespace MySpot.Api.Exceptions;

public class ParkingSpotAlreadyReservedException(ParkingSpotName name, Date date): MySpotException($"Parking spot: {name} already reserved at: {date:d}.")
{
    
}