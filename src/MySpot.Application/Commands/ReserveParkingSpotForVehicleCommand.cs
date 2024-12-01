namespace MySpot.Application.Commands;

public record ReserveParkingSpotForVehicleCommand(Guid ParkingSpotId, Guid ReservationId, string EmployeeName, string LicensePlate, DateTime Date);