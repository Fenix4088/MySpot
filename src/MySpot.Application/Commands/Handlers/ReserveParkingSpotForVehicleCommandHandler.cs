using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public class ReserveParkingSpotForVehicleCommandHandler(IClock clock, IWeeklyParkingSpotRepository weeklyParkingSpotRepositoryRepository, IParkingReservationService parkingReservationService): ICommandHandler<ReserveParkingSpotForVehicleCommand>
{
    private readonly IClock _clock = clock;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository = weeklyParkingSpotRepositoryRepository;
    private readonly IParkingReservationService _parkingReservationService = parkingReservationService;
    
    public async Task HandleAsync(ReserveParkingSpotForVehicleCommand command)
    {
        var week = new Week(_clock.Current());
        var parkingSpotId = new ParkingSpotId(command.ParkingSpotId);
        var weeklyParkingSpots = (await _weeklyParkingSpotRepository.GetByWeekAsync(week)).ToList();
        var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(spot => spot.Id == parkingSpotId);

        if (parkingSpotToReserve is null)
        {
            throw new WeeklyParkingSpotNotFoundException(parkingSpotId);
        }

        var reservation = new VehicleReservation(
            command.ReservationId,
            command.ParkingSpotId,
            new Date(command.Date),
            command.EmployeeName,
            command.LicensePlate,
            command.Capacity
        );
        
        //TODO: refactor, hardcoded JobTitle.Employee policy
        _parkingReservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, parkingSpotToReserve, reservation);
        await _weeklyParkingSpotRepository.UpdateAsync(parkingSpotToReserve);
    }
}