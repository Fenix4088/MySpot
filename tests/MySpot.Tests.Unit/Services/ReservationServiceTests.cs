using MySpot.Application.Commands;
using MySpot.Application.Services;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Policies;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.DAL.Repositories;
using MySpot.Tests.Unit.Shared;
using Shouldly;
using Xunit;

namespace MySpot.Tests.Unit.Services;

public class ReservationServiceTests
{
    #region Arrange

    private readonly IClock _clock;
    private readonly IReservationsService _reservationsService;
    private readonly IWeeklyParkingSpotRepository _weeklyParkingSpotRepository;
    private readonly IParkingReservationService _parkingReservationService;
    
    public ReservationServiceTests()
    {
        _clock = new TestClock();
        var policies = new List<IReservationPolicy>() { 
            new BossReservationPolicy(),
            new ManagerReservationPolicy(_clock),
            new RegularEmployeeReservationPolicy(_clock)
        };
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _parkingReservationService = new ParkingReservationService(policies, _clock);
        _reservationsService = new ReservationsService(_clock, _weeklyParkingSpotRepository, _parkingReservationService);
    }
    #endregion

    [Fact]
    public async Task give_reservation_for_not_taken_date_create_reservation_should_succeed()
    {

        var parkingSpots = await _weeklyParkingSpotRepository.GetAllAsync();
        var parkingSpot = parkingSpots.First();
        var command = new ReserveParkingSpotForVehicleCommand(
            parkingSpot.Id,
            Guid.NewGuid(),
            "Joe Dou",
            "XYZ123",
            DateTime.UtcNow.AddMinutes(5)
            );

        var reservationId = await _reservationsService.ReserveForVehicleAsync(command);

        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }
}