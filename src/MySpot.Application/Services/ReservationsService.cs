using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Services;

public class ReservationsService(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepositoryRepository, IParkingReservationService parkingReservationService): IReservationsService
{
    private readonly IClock _clock = clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository = weeklyParkingSpotRepositoryRepository;
    private readonly IParkingReservationService _parkingReservationService = parkingReservationService;

    public async Task<IEnumerable<ReservationDto>> GetAllWeeklyAsync()
    {
        var weeklyParkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
        
        return weeklyParkingSpots.SelectMany(weeklyParkingSpot => weeklyParkingSpot.Reservations).Select(reservation =>
            new ReservationDto()
            {
                Id = reservation.Id,
                EmployeeName = reservation.EmployeeName,
                Date = reservation.Date.Value.Date,
                ParkingSpotId = reservation.ParkingSpotId
            });
    }

    public async Task<ReservationDto?> GetAsync(Guid id)
    {
        var reservations = await GetAllWeeklyAsync();
        return reservations.SingleOrDefault(r => r.Id == id);
    }

    public async Task<Guid?> CreateAsync(CreateReservationCommand createReservationCommand)
    {
        var week = new Week(_clock.Current());
        var parkingSpotId = new ParkingSpotId(createReservationCommand.ParkingSpotId);
        var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetByWeekAsync(week)).ToList();
        var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(spot => spot.Id == parkingSpotId);

        if (parkingSpotToReserve is null)
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
//TODO: refactor, hardcoded JobTitle.Employee policy
        _parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, parkingSpotToReserve, reservation);
        await _weeklyParkingSpotRepository.UpdateAsync(parkingSpotToReserve);

        return reservation.Id;
    }

    public async Task<bool> UpdateAsync(ChangeReservationLicensePlateCommand changeReservationLicensePlateCommand)
    {
        
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(changeReservationLicensePlateCommand.ReservationId);
        
        if (weeklyParkingSpot is null) return false;
        
        var existingReservation =
            weeklyParkingSpot.Reservations.SingleOrDefault(reservation =>
                reservation.Id == new ReservationId(changeReservationLicensePlateCommand.ReservationId));

        if (existingReservation is null) return false;

        if (existingReservation.Date <= new Date(_clock.Current())) return false;
        
        existingReservation.ChangeLicensePlate(changeReservationLicensePlateCommand.LicensePlate);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);

        return true;
    }

    public async Task<bool> DeleteAsync(DeleteReservationCommand deleteReservationCommand)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(deleteReservationCommand.ReservationId);
        
        if (weeklyParkingSpot is null) return false;
        
        var existingReservation =
            weeklyParkingSpot.Reservations.SingleOrDefault(reservation =>
                reservation.Id == new ReservationId(deleteReservationCommand.ReservationId));
        
        if (existingReservation is null) return false;
        
        weeklyParkingSpot.RemoveReservation(deleteReservationCommand.ReservationId);
        await _weeklyParkingSpotRepository.DeleteAsync(weeklyParkingSpot);

        return true;
    }

    private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await  _weeklyParkingSpotRepository.GetAllAsync();
        return weeklyParkingSpots.SingleOrDefault(parkingSpot => parkingSpot.Reservations.Any(reservation => reservation.Id == new ReservationId(reservationId)));
    }

}