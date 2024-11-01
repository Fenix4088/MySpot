namespace MySpot.Core.ValueObjects;

public sealed record ParkingSpotId(Guid value)
{
    public Guid Value { get; } = value;

    public static implicit operator Guid(ParkingSpotId parkingSpotId) => parkingSpotId.Value;
    public static implicit operator ParkingSpotId(Guid parkingSpotId) => new(parkingSpotId);
}