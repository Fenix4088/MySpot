using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Services;

public class ReservationsService
{
    private static readonly Clock _clock = new();
    
    private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots = [
        new WeeklyParkingSpot(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            new Week(_clock.Current()),
            "P1"
            ),
        new WeeklyParkingSpot(
            Guid.Parse("00000000-0000-0000-0000-000000000002"),
            new Week(_clock.Current()),
            "P2"
        ),
        new WeeklyParkingSpot(
            Guid.Parse("00000000-0000-0000-0000-000000000003"),
            new Week(_clock.Current()),
            "P3"
        ),
        new WeeklyParkingSpot(
            Guid.Parse("00000000-0000-0000-0000-000000000004"),
            new Week(_clock.Current()),
            "P4"
        ),
        new WeeklyParkingSpot(
            Guid.Parse("00000000-0000-0000-0000-000000000005"),
            new Week(_clock.Current()),
            "P5"
        )
    ];


    public IEnumerable<ReservationDto> GetAllWeekly() => WeeklyParkingSpots.SelectMany(weeklyParkingSpot => weeklyParkingSpot.Reservations).Select(reservation => new ReservationDto()
    {
        Id = reservation.Id,
        EmployeeName = reservation.EmployeeName,
        Date = reservation.Date.Value.Date,
        ParkingSpotId = reservation.ParkingSpotId
    });
    
    public ReservationDto? Get(Guid id) => GetAllWeekly().SingleOrDefault(r => r.Id == id);

    public Guid? Create(CreateReservationCommand createReservationCommand)
    {
        var weeklyParkingSpot = WeeklyParkingSpots.SingleOrDefault(spot => spot.Id == new ParkingSpotId(createReservationCommand.ParkingSpotId));

        if (weeklyParkingSpot is null)
        {
            return default;
        }

        var reservation = new Reservation(
            createReservationCommand.ReservationId,
            createReservationCommand.ParkingSpotId,
            createReservationCommand.EmployeeName,
            createReservationCommand.LicensePlate,
            new Date(createReservationCommand.Date)
            );

        weeklyParkingSpot.AddReservation(reservation, new Date(_clock.Current()));

        return reservation.Id;
    }

    public bool Update(ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand)
    {
        
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(changeReservationLicensePlateCommand.ReservationId);
        
        if (weeklyParkingSpot is null) return false;
        
        var existingReservation =
            weeklyParkingSpot.Reservations.SingleOrDefault(reservation =>
                reservation.Id == new ReservationId(changeReservationLicensePlateCommand.ReservationId));

        if (existingReservation is null) return false;

        if (existingReservation.Date <= new Date(_clock.Current())) return false;
        
        existingReservation.ChangeLicensePlate(changeReservationLicensePlateCommand.LicensePlate);
        
        return true;
    }

    public bool Delete(DeleteReservationCommand deleteReservationCommand)
    {
        var weeklyParkingSpot = GetWeeklyParkingSpotByReservation(deleteReservationCommand.ReservationId);
        
        if (weeklyParkingSpot is null) return false;
        
        var existingReservation =
            weeklyParkingSpot.Reservations.SingleOrDefault(reservation =>
                reservation.Id == new ReservationId(deleteReservationCommand.ReservationId));
        
        if (existingReservation is null) return false;
        
        weeklyParkingSpot.RemoveReservation(deleteReservationCommand.ReservationId);
        
        return true;
    }

    private WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(Guid reservationId) => WeeklyParkingSpots.SingleOrDefault(parkingSpot => parkingSpot.Reservations.Any(reservation => reservation.Id == new ReservationId(reservationId)));

}