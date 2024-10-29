namespace MySpot.Api.Commands;

public record CreateReservationCommand(Guid ParkingSpotId, Guid ReservationId, string EmployeeName, string LicensePlate, DateTime Date);