using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name)
{
    private readonly HashSet<Reservation> _reservations = new();

    public ParkingSpotId Id { get; private set; } = id;
    public Week Week { get; private set; } = week;
    public ParkingSpotName Name { get; private set; } = name;
    public IEnumerable<Reservation> Reservations => _reservations;

    public void AddReservation(Reservation reservation, Date now)
    {

        var reservationDate = reservation.Date;
        var isInvalidDate = reservationDate < Week.From || reservationDate > Week.To || reservationDate < now;
        
        if (isInvalidDate) throw new InvalidReservationDateException(reservation.Date.Value.Date);
        
        if (_reservations.Any(r => r.Date == reservation.Date)) throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);

        _reservations.Add(reservation);
    }

    public void RemoveReservation(ReservationId reservationId)
    {
        _reservations.RemoveWhere(reservation => reservation.Id == reservationId);
    }
}