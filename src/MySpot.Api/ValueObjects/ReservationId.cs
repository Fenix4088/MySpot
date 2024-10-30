namespace MySpot.Api.ValueObjects;

public sealed record ReservationId(Guid value)
{
    public Guid Value { get; } = value;

    public static implicit operator Guid(ReservationId reservationId) => reservationId.Value;
    public static implicit operator ReservationId(Guid reservationId) => new(reservationId);
}