using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class ChangeReservationLicensePlateCommandHandler(IWeeklyParkingSpotRepository weeklyParkingSpotRepository): ICommandHandler<ChangeReservationLicensePlateCommand>
{
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository = weeklyParkingSpotRepository;
    
    public async Task HandleAsync(ChangeReservationLicensePlateCommand command)
    {
        var weeklyParkingSpot = await GetWeeklyParkingSpotByReservationAsync(command.ReservationId);

        if (weeklyParkingSpot is null)
        {
            throw new WeeklyParkingSpotNotFoundException();
        }
        
        var existingReservation =
            weeklyParkingSpot.Reservations.OfType<VehicleReservation>().SingleOrDefault(reservation =>
                reservation.Id == new ReservationId(command.ReservationId));

        if (existingReservation is null) {
            throw new ReservationNotFoundException(command.ReservationId);
        }

        
        existingReservation.ChangeLicensePlate(command.LicensePlate);
        await _weeklyParkingSpotRepository.UpdateAsync(weeklyParkingSpot);

    }
    
    private async Task<WeeklyParkingSpot?> GetWeeklyParkingSpotByReservationAsync(Guid reservationId)
    {
        var weeklyParkingSpots = await  _weeklyParkingSpotRepository.GetAllAsync();
        return weeklyParkingSpots.SingleOrDefault(parkingSpot => parkingSpot.Reservations.Any(reservation => reservation.Id == new ReservationId(reservationId)));
    }
    
}