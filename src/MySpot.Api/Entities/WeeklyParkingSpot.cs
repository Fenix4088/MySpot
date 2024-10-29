using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot(Guid id, DateTime from, DateTime to, string name)
{
    private readonly HashSet<Reservation> _reservations = new();

    public Guid Id { get; } = id;
    public DateTime From { get; } = from;
    public DateTime To { get; } = to;
    public string Name { get; } = name;
    public IEnumerable<Reservation> Reservations => _reservations;

    public void AddReservation(Reservation reservation, DateTime now)
    {

        var reservationDate = reservation.Date.Date;
        var isInvalidDate = reservationDate < From || reservationDate > To || reservationDate < now.Date;
        
        if (isInvalidDate) throw new InvalidReservationDateException(reservation.Date.Date);
        
        if (_reservations.Any(r => r.Date == reservation.Date)) throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);

        _reservations.Add(reservation);
    }

    public void RemoveReservation(Guid reservationId)
    {
        _reservations.RemoveWhere(reservation => reservation.Id == reservationId);
    }
}