using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class ParkingSpotCapacityExceededException(ParkingSpotId parkingSpotId): MySpotException($"Parking spot with id {parkingSpotId} exceeds its reservation capacity.")
{
    
}