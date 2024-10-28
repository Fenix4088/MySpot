using MySpot.Api.Models;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static int Id = 1;
    private static readonly List<string> ParkingSpotNames = [
        "P1",
        "P2",
        "P3",
        "P4",
        "P5",
    ];
    private static readonly List<Reservation> ReservationsList = [];


    public IEnumerable<Reservation> GetAll() => ReservationsList;
    
    public Reservation? Get(int id) => ReservationsList.SingleOrDefault(r => r.Id == id);

    public int? Create(Reservation reservation)
    {
        if (ParkingSpotNames.All(spot => spot != reservation.ParkingSpotName))
        {
            return default;
        }

        if (ReservationsList.Any(r => r.Date == reservation.Date && r.ParkingSpotName == reservation.ParkingSpotName))
        {
            return default;
        }

        reservation.Id = Id;
        Id++;
        ReservationsList.Add(reservation);

        return reservation.Id;
    }

    public bool Update(int id, Reservation newReservation)
    {
        var reservation = ReservationsList.SingleOrDefault(r => r.Id == id);

        if (reservation is null)
        {
            return false;
        }
        
        if (ParkingSpotNames.All(spot => spot != newReservation.ParkingSpotName))
        {
            return false;
        }
        
        var reservationIndex = ReservationsList.IndexOf(reservation);
        ReservationsList[reservationIndex] = newReservation;
        ReservationsList[reservationIndex].Id = id;
        return true;
    }

    public bool Delete(int id)
    {
        var reservation = ReservationsList.SingleOrDefault(r => r.Id == id);
        if (reservation is null)
        {
            return false;
        }
        return ReservationsList.Remove(reservation);
    }

}