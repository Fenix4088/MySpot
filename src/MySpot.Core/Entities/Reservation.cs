using MySpot.Core.ValueObjects;

namespace MySpot.Core.Entities;

public abstract class Reservation
{
    public ReservationId Id { get; private set;  }
    public ParkingSpotId ParkingSpotId { get; }

    public Capacity Capacity { get; private set; }
    public Date Date { get; private set; }
    
    protected Reservation()
    {
    }

    public Reservation(ReservationId id,
        ParkingSpotId parkingSpotId,
        Capacity capacity,
        Date date)
    {
        Id = id;
        ParkingSpotId = parkingSpotId;
        Capacity = capacity;
        Date = date;
    }

    
}