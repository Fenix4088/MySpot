namespace MySpot.Api.Exceptions;

public class ParkingSpotAlreadyReservedException(string name, DateTime date): MySpotException($"Parking spot: {name} already reserved at: {date:d}.")
{
    
}