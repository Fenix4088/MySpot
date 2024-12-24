using MySpot.Core.Entities;
using MySpot.Core.Exceptions;
using MySpot.Core.ValueObjects;
using Shouldly;
using Should;
using Xunit;

namespace MySpot.Tests.Unit.Entities;

public class WeeklyParkingSpotTests
{

    #region Arrange
    
    private readonly WeeklyParkingSpot _weeklyParkingSpot;
    private readonly Date _now;

    public WeeklyParkingSpotTests()
    {
        _now = new Date(new DateTime(2024, 10, 10));
        _weeklyParkingSpot = WeeklyParkingSpot.Create(Guid.NewGuid(), new Week(_now), "P1");
    }
    
    #endregion
    
    [Theory]
    [InlineData("2024-12-11")]
    [InlineData("2035-12-10")]
    public void given_invalid_date_add_reservation_should_fail(string dateString)
    {
        //arrange
        var invalidDate = DateTime.Parse(dateString);
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            new Date(invalidDate),
            "Joe Dou",
            "XYZ123",
            1
            );
        
        //act
        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation, new Date(_now)));
        
        //assert
        
        //Alternative for Assert, Required install new Package `Should`
        exception.ShouldNotBeNull();
        exception.ShouldBeType<InvalidReservationDateException>();
        
        // Assert.NotNull(exception);
        // Assert.IsType<InvalidReservationDateException>(exception);
    }
    
    [Fact]
    public void given_reservation_for_already_given_existing_date_add_reservation_should_fail()
    {
        //arrange
        var reservationDate = new DateTime(2024, 10, 11);
        var reservation_1 = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            new Date(reservationDate),
            "Joe Dou",
            "XYZ123",
            2
        );
        
        var reservation_2 = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            new Date(reservationDate),
            "Joe Dou 2",
            "XYZ124",
            1
        );
        
        //act
        _weeklyParkingSpot.AddReservation(reservation_1, new Date(_now));
        var exception = Record.Exception(() => _weeklyParkingSpot.AddReservation(reservation_2, new Date(_now)));

        //assert
        exception.ShouldNotBeNull();
        exception.ShouldBeType<ParkingSpotCapacityExceededException>();
    }
    
    [Fact]
    public void given_reservation_for_not_reserved_parking_spot_add_reservation_should_succeed()
    {
        //arrange
        var reservationDate = new DateTime(2024, 10, 11);
        var reservation = new VehicleReservation(
            Guid.NewGuid(),
            _weeklyParkingSpot.Id,
            new Date(reservationDate),
            "Joe Dou",
            "XYZ123",
            2
        );
        
        //act
        _weeklyParkingSpot.AddReservation(reservation, _now);

        //assert
        _weeklyParkingSpot.Reservations.ShouldContain(reservation);
    }
}