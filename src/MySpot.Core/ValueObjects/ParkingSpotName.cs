using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public sealed record ParkingSpotName(string value)
{
    public string Value { get; } = value ?? throw new InvalidParkingSpotNameException();

    public static implicit operator string(ParkingSpotName parkingSpotName) => parkingSpotName.Value;
    public static implicit operator ParkingSpotName(string parkingSpotName) => new(parkingSpotName);
};