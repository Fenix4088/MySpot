using MySpot.Api.Commands;
using MySpot.Api.Repositories;
using MySpot.Api.Services;
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
    
    public ReservationServiceTests()
    {
        _clock = new TestClock();
        _weeklyParkingSpotRepository = new InMemoryWeeklyParkingSpotRepository(_clock);
        _reservationsService = new ReservationsService(_clock, _weeklyParkingSpotRepository);
    }
    #endregion

    [Fact]
    public void give_reservation_for_not_taken_date_create_reservation_should_succeed()
    {

        var parkingSpot = _weeklyParkingSpotRepository.GetAll().First();
        var command = new CreateReservationCommand(
            parkingSpot.Id,
            Guid.NewGuid(),
            "Joe Dou",
            "XYZ123",
            DateTime.UtcNow.AddMinutes(5)
            );

        var reservationId = _reservationsService.Create(command);

        reservationId.ShouldNotBeNull();
        reservationId.Value.ShouldBe(command.ReservationId);
    }
}