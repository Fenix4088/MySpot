using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepositoryRepository): IReservationsService
{
    private readonly IClock _clock = clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository = weeklyParkingSpotRepositoryRepository;
    
    public IEnumerable<ReservationDto> GetAllWeekly() => _weeklyParkingSpotRepository.GetAll().SelectMany(weeklyParkingSpot => weeklyParkingSpot.Reservations).Select(reservation => new ReservationDto()
    {
        Id = reservation.Id,
        EmployeeName = reservation.EmployeeName,
        Date = reservation.Date.Value.Date,
        ParkingSpotId = reservation.ParkingSpotId
    });
    
    public ReservationDto? Get(Guid id) => GetAllWeekly().SingleOrDefault(r => r.Id == id);

    public Guid? Create(CreateReservationCommand createReservationCommand)
    {
        var weeklyParkingSpot = _weeklyParkingSpotRepository.GetAll().SingleOrDefault(spot => spot.Id == new ParkingSpotId(createReservationCommand.ParkingSpotId));

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
        _weeklyParkingSpotRepository.Update(weeklyParkingSpot);

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
        _weeklyParkingSpotRepository.Update(weeklyParkingSpot);

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
        _weeklyParkingSpotRepository.Delete(weeklyParkingSpot);

        return true;
    }

    private WeeklyParkingSpot? GetWeeklyParkingSpotByReservation(Guid reservationId) => _weeklyParkingSpotRepository.GetAll().SingleOrDefault(parkingSpot => parkingSpot.Reservations.Any(reservation => reservation.Id == new ReservationId(reservationId)));

}