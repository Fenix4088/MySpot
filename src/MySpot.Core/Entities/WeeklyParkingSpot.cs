using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public class WeeklyParkingSpot
{
    private readonly HashSet<Reservation> _reservations = new();
    public const int MaxCapacity = 2;

    public ParkingSpotId Id { get; private set; }
    public Week Week { get; private set; }
    public ParkingSpotName Name { get; private set; }
    public IEnumerable<Reservation> Reservations => _reservations;

    public Capacity Capacity { get; private set; }
    
    private WeeklyParkingSpot(ParkingSpotId id, Week week, ParkingSpotName name, Capacity capacity)
    {
        Id = id;
        Week = week;
        Name = name;
        Capacity = capacity;
    }

    public static WeeklyParkingSpot Create(ParkingSpotId id, Week week, ParkingSpotName name) => new(id, week, name, MaxCapacity);

    internal void AddReservation(Reservation reservation, Date now)
    {

        var reservationDate = reservation.Date;
        var isInvalidDate = reservationDate < Week.From || reservationDate > Week.To || reservationDate < now;
        
        if (isInvalidDate) throw new InvalidReservationDateException(reservation.Date.Value.Date);
        

        var dateCapacity = _reservations
            .Where(x => x.Date == reservationDate)
            .Sum(x => x.Capacity);
        
        if (dateCapacity + reservation.Capacity > Capacity)
        {
            throw new ParkingSpotCapacityExceededException(Id);
        }

        _reservations.Add(reservation);
    }

    public void RemoveReservation(ReservationId reservationId)
    {
        _reservations.RemoveWhere(reservation => reservation.Id == reservationId);
    }

    public void RemoveReservations(IEnumerable<Reservation> reservations) =>
        _reservations.RemoveWhere(x => reservations.Any(r => r.Id == x.Id));
}