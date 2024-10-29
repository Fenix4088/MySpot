using MySpot.Api.Exceptions;

namespace MySpot.Api.Entities;

public class WeeklyParkingSpot(Guid id, DateTime from, DateTime to, string name)
{
    private readonly HashSet<Reservation> _reservetions = new();

    public Guid Id { get; } = id;
    public DateTime From { get; } = from;
    public DateTime To { get; } = to;
    public string Name { get; } = name;
    public IEnumerable<Reservation> Reservations => _reservetions;

    public void AddReservation(Reservation reservation)
    {

        var reservationDate = reservation.Date.Date;
        var now = DateTime.UtcNow.Date;
        var isInvalidDate = reservationDate < From || reservationDate > To || reservationDate < now.Date;
        
        if (isInvalidDate) throw new InvalidReservationDateException(reservation.Date.Date);
        
        if (_reservetions.Any(r => r.Date == reservation.Date)) throw new ParkingSpotAlreadyReservedException(Name, reservation.Date);

        _reservetions.Add(reservation);
    }
}