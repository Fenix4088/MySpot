using MySpot.Core.ValueObjects;

namespace MySpot.Core.Exceptions;

public sealed class CanNotReserveParkingSpotException(ParkingSpotId parkingSpotId) : MySpotException($"Can not reserve parking spot with id: {parkingSpotId}")
{
    public ParkingSpotId ParkingSpotId = parkingSpotId;
}